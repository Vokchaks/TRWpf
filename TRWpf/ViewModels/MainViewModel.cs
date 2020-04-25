using TRWpf.Models;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using TRWpf.Views;
using System.Windows.Controls;
using TRWpf.DataAccess;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO.Ports;
using System.Diagnostics;

namespace TRWpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

      //  ObservableCollection<Card> Cards { get; set; }
        private ObservableCollection<Card> _cards;
        ObservableCollection<Card> Cards
        {
            get { return _cards; }
            set
            {
                _cards = value;
                OnPropertyChanged("Cards");
            }
        }
        ObservableCollection<User> Users { get; set; }

        string _btnConnectLabel = "Подключить";
        public string BtnConnectLabel
        {
            get { return _btnConnectLabel; }
            set
            {
                _btnConnectLabel = value;
                OnPropertyChanged("BtnConnectLabel");
            }
        }

        bool _comPresent = false;
        public bool ComPresent
        {
            get { return _comPresent; }
            set
            {
                _comPresent = value;
                OnPropertyChanged("ComPresent");
            }
        }

        ContentControl _workContent;
        public ContentControl WorkContent { 
            get { return _workContent; } 
            set 
            {
                _workContent = value;
                OnPropertyChanged("WorkContent");
            }               
        }
        ContentControl UsersControl, CardsControl;

        ComPort ComX;

        private string _statusText;
        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }
    
        public MainViewModel()
        {

            StatusText = "Статус строка";
            Users = UserRepository.Users;
            Cards = CardRepository.Cards;
            foreach (Card c in Cards)
            {
                if (c.User_Id > 0)
                    c.User = Users.FirstOrDefault(x => x.User_Id == c.User_Id);
            }

            var uvm = new UsersViewModel (Users) { Cards = Cards };
            var cvm = new CardsViewModel { Cards = Cards };

            UsersControl = new UsersView { DataContext = uvm };
            CardsControl = new CardsView { DataContext = cvm };
  
            Messenger.Default.Register<StatusMessage>(this, (action) =>
            {
                StatusText = action.Msg;
            });

            ComsList = new ObservableCollection<Element>();
      
            int i = 0;
            foreach(string str in SerialPort.GetPortNames())
            {
                if (str != null)
                {
                    Element element = new Element { Id = i, Name = str };
                    ComsList.Add(element);
                    if (i == 0)
                        SelectedCom = element;
                }
                i++;
            }
            if (ComsList.Count > 0) ComPresent = true;

            //WorkContent = CardsControl;
            
            WorkContent = UsersControl;
        }

        Element _selectedCom;
        public Element SelectedCom
        {
            get { return _selectedCom; }
            set { 
                _selectedCom = value;
                OnPropertyChanged("SelectedCom");
            }
        }

        ICommand _cardsViewCommand;
        public ICommand CardsViewCommand
        {
            get
            {
                return _cardsViewCommand ??= new RelayCommand(param => {
                    WorkContent = CardsControl;
                });
            }
        }

        ICommand _connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                return _connectCommand ??= new RelayCommand(param =>
                {
                    Debug.WriteLine(SelectedCom.Name);

                    if (ComX == null)
                        ComX = new ComPort(SelectedCom.Name);
                    if (ComX.IsOpen)
                        BtnConnectLabel = "Отключить";
                    else
                    {
                        ComX.Close();
                        BtnConnectLabel = "Подключить";
                    }
                });
            }
        }

        ICommand _usersViewCommand;
        public ICommand UsersViewCommand
        {
            get
            {
                return _usersViewCommand ??= new RelayCommand(param => {
                    WorkContent = UsersControl;
                });
            }
        }

        public ObservableCollection<Element> ComsList{ get; private set; }
    }
}
