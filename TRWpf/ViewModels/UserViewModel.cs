using System.Collections.ObjectModel;
using TRWpf.Models;
using TRWpf.DataAccess;
using System.ComponentModel;
using System.Windows.Data;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.IO;
using System.Globalization;

namespace TRWpf.ViewModels
{
    public class UserViewModel : ViewModelBase 
    {
        public ObservableCollection<Acc_Level> accLevels { get; set; }

        public ObservableCollection<Card> Cards { get; set; }

        ObservableCollection<Card> _userCards;
        public ObservableCollection<Card> UserCards
        {
            get { return _userCards; }
            set
            {
                _userCards = value;
                OnPropertyChanged("UserCards");
            }
        }

        ICommand _closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ??= new RelayCommand(param => {

                    if (param as Window != null)
                        (param as Window).Close();
                });
            }
        }

        ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ??= new RelayCommand(param => SaveUser());
            }
        }

        void SaveUser()
        {
            
            Debug.WriteLine($"SaveUser:  {user.Full_Name}");
            UserRepository repositrory = new UserRepository();
            if (user.User_Id == 0)
            {
                user.User_Id = repositrory.Create(user);
                user_Photo.User_Id = user.User_Id;
            }
            else
                repositrory.Update(user);
                        
            if (user_Photo.Change)
            {
                User_PhotoRepository upr = new User_PhotoRepository();
                if(user_Photo.Photo != null && user_Photo.Photo.Length > 0 )
                    upr.UpdateOrInsert(user_Photo);
                else
                {
                    User_Photo photo = upr.GetById(user_Photo.User_Id);
                    if (photo != null)
                        upr.Delete(user_Photo.User_Id);
                }
            }

            
            if (UserCards != null && UserCards.Count > 0)
            {
                CardRepository cr = new CardRepository();
                foreach (var c in UserCards)
                {
                    Card card = Cards.FirstOrDefault(c => c.Card_Num == SelectedCard.Card_Num);
                    card = c;
                    cr.UpdateOrInsert(c);
                }
            }
        }

        ICommand _changeStatusCardCommand;
        public ICommand ChangeStatusCardCommand
        {
            get
            {
                return _changeStatusCardCommand ??= new RelayCommand(param =>
                {
                    if (SelectedCard == null)
                        return;
                    SelectedCard.Status = int.Parse((string)param, CultureInfo.InvariantCulture);
                    Debug.WriteLine($"STATUS: { SelectedCard.Status}");
                    if (SelectedCard.Status == 1)
                    {
                        SelectedCard.User_Id = user.User_Id;
                        SelectedCard.User = user;
                    }
                    else
                    {
                        SelectedCard.User_Id = null;
                        SelectedCard.User = null;
                        CardRepository cr = new CardRepository();
                        cr.UpdateOrInsert(SelectedCard);
                        UserCards.Remove(SelectedCard);
                        SelectedCard = null;
                    }
                });
            }
        }

        ICommand _okCommand;
        public ICommand OkCommand
        {
            get
            {
                return _okCommand ??= new RelayCommand(param => {
                    SaveUser();
                    if (param is Window window)
                        window.Close();
                });
            }
        }

        ICommand _loadPhotoCommand;
        public ICommand LoadPhotoCommand
        {
            get
            {
                return _loadPhotoCommand ??= new RelayCommand(param =>
                {
                    OpenFileDialog open = new OpenFileDialog
                    {
                        DefaultExt = (".jpg"),
                        Filter = "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png"
                    };

                    if (open.ShowDialog() == true)
                    {
                        FileInfo file = new FileInfo(open.FileName);
                        if ( file.Length > 300 * 1024)
                        {
                            MessageBox.Show("Размер фото не должен превышать 300К.", String.Intern("Ошибка"), MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        user_Photo.Photo = File.ReadAllBytes(open.FileName);
                        user_Photo.Change = true;
                    }
                });
            }
        }

        ICommand _deletePhotoCommand;
        public ICommand DeletePhotoCommand
        {
            get
            {
                return _deletePhotoCommand ??= new RelayCommand(param =>
                {
                    if (user_Photo.Photo == null)
                        return;
                    user_Photo.Photo = null;
                    user_Photo.Change = true;

                });
            }
        }

        public IEnumerable<Element> CardTypeViewModels { get; private set; }
        public IEnumerable<Element> UserGroupViewModels { get; private set; }
        public IEnumerable<Element> CardStatusViewModels { get; private set; }
       
        User _user;
        public User user
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("user");
            }
        }

        public void InitUser()
        {
            accLevels = Acc_LevelRepository.Acc_Levels;

            Debug.WriteLine($"User Acc_Lvl:  {user.Acc_Lvl}");

            if (user.User_Id > 0)
                UserCards = new ObservableCollection<Card>(Cards.Where(c => c.User_Id == user.User_Id).Select(c => c));

            user_Photo = new User_Photo { User_Id = user.User_Id };

            Debug.WriteLine($"User ID:  {user.User_Id}");

            if (user.User_Id < 1)
                return;

            User_PhotoRepository upr = new User_PhotoRepository();
            User_Photo up = upr.GetById(user.User_Id);
            if (up == null || up.Photo == null)
                return;

            Debug.WriteLine($"Return User ID:  {up.User_Id}");
            Debug.WriteLine($"Not null User Photo:  {up.Photo.Length}");

            user_Photo.Photo = up.Photo;
        }

        User_Photo _user_Photo;
        public User_Photo user_Photo
        {
            get { return _user_Photo; }
            set
            {
                _user_Photo = value;
                OnPropertyChanged("user_Photo");
            }
        }

        Card _selectedCard = null;
        public Card SelectedCard
        {
            get { return _selectedCard; }
            set
            {
                _selectedCard = value;
                OnPropertyChanged("SelectedCard");
                string str = _selectedCard == null ? "Не выбрана" : "Выбрана карта: " + _selectedCard.Card_Num.ToString(CultureInfo.InvariantCulture);
                Messenger.Default.Send<StatusMessage>(new StatusMessage { Msg = str });
                _selectedCard?.Log("Selected card data");
            }
        }

        public ICollectionView cardsView
        {
            get
            {
                Debug.WriteLine($"cards count: {Cards.Count}");
                return CollectionViewSource.GetDefaultView(Cards);
            }
        }

        public UserViewModel()
        {
            Debug.WriteLine($"INIT USER VIEW MODEL");
            CardTypeViewModels = Element.CardTypeViewModels;
            CardStatusViewModels = Element.CardStatusViewModels;
            UserGroupViewModels = Element.UserGroupViewModels;
            Messenger.Default.Register<ComMessage>(this, (action) => LineReceived(action));
        }

        private void LineReceived(ComMessage data)
        {
            Debug.WriteLine("Пллучили карту: " + data.Msg);
            Card card = UserCards.FirstOrDefault(c => c.Card_Num.ToString(CultureInfo.InvariantCulture)
                .StartsWith(data.Msg, StringComparison.OrdinalIgnoreCase));
            if (card != null)
            {
                SelectedCard = card;
                return;
            }
            card = Cards.FirstOrDefault(c => c.Card_Num.ToString(CultureInfo.InvariantCulture)
                .StartsWith(data.Msg, StringComparison.InvariantCulture));
            if (card != null)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    UserCards.Add(card);
                });
                SelectedCard = card;
                return;
            }
            var msg = new StatusMessage { Msg = "Не Найдена: " + data.Msg };
            Messenger.Default.Send<StatusMessage>(msg);
        }

        ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                return _loadedCommand ??= new RelayCommand(param =>
                {
                    Messenger.Default.Register<ComMessage>(this, (action) => LineReceived(action));
                    Debug.WriteLine("USER LOADED");
                });
            }
        }

        ICommand _unloadedCommand;
        public ICommand UnloadedCommand
        {
            get
            {
                return _unloadedCommand ??= new RelayCommand(param =>
                {
                    Messenger.Default.Unregister<ComMessage>(this, (action) => LineReceived(action));
                    Debug.WriteLine("USER UN-LOADED");                    
                });
            }
        }

        //public bool Filter(Card card)
        //{

        //    if (card == null)
        //        return true;

        //    if (card.User_Id == user.User_Id)
        //        return true;
        //    else
        //        return false;
        //}
    }
}