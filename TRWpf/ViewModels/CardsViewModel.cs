using TRWpf.Models;
using TRWpf.DataAccess;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Globalization;

namespace TRWpf.ViewModels
{
    public class CardsViewModel : ViewModelBase
    {
        private CardRepository repository;
        bool _loaded = false;
        // 
        public ObservableCollection<Card> Cards { get; set; }

        public IEnumerable<Element> CardTypeViewModels { get; private set; }
        public IEnumerable<Element> CardStatusViewModels { get; private set; }


        Card _selectedCard = null;
        public  Card SelectedCard
        {
            get { return _selectedCard; }
            set
            {
                if (!_loaded)
                    return;
                _selectedCard = value;
                OnPropertyChanged("SelectedCard");
                string str = _selectedCard == null ? "Не выбрана" : "Выбрана карта: " + _selectedCard.Card_Num.ToString(CultureInfo.InvariantCulture);
                Messenger.Default.Send<StatusMessage>( new StatusMessage { Msg = str });
                _selectedCard?.Log("Selected card data");
            }
        }

        string _patternCard = null;
        public string PatternCard
        {
            get { return _patternCard; }
            set
            {
                _patternCard = value;
                if (! String.IsNullOrEmpty(_patternCard))
                    SelectedCard = Cards.FirstOrDefault(c => c.Card_Num.ToString(CultureInfo.InvariantCulture).StartsWith(PatternCard, StringComparison.InvariantCultureIgnoreCase));
                SelectedCard = null;
                OnPropertyChanged("PatternCard");
            }
        }

        public CardsViewModel()
        {
            CardTypeViewModels = Element.CardTypeViewModels;
            CardStatusViewModels = Element.CardStatusViewModels;
            repository = new CardRepository();
        }

        private void ComReceived(ComMessage data)
        {
            Debug.WriteLine("Пллучили карту: " + data.Msg);
            Card card = Cards.FirstOrDefault(c => c.Card_Num.ToString(CultureInfo.InvariantCulture).StartsWith(data.Msg,StringComparison.InvariantCultureIgnoreCase));
            if (card == null)
            {
                MessageBoxResult result =
                    MessageBox.Show("Новая карта: " + data.Msg + ". Добавить?", String.Intern("Новая карта"), MessageBoxButton.YesNo,
                    MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    Debug.WriteLine($"Добавим карту");
                    card = new Card
                    {
                        Card_Num = int.Parse(data.Msg,CultureInfo.InvariantCulture)
                    };
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Cards.Add(card);
                    });
                    SelectedCard = card;
                    SaveCardToDb();
                }
                return;
            }
            else
            {
                PatternCard = data.Msg;
                var msg = new StatusMessage { Msg = "Найдена: " + data.Msg };
                Messenger.Default.Send<StatusMessage>(msg);
            }
        }

        private void SaveCardToDb()
        {
            if (SelectedCard == null)
                return;
            repository.UpdateOrInsert(SelectedCard);
        }

        ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                return _loadedCommand ??= new RelayCommand(param =>
                {
                    Messenger.Default.Register<ComMessage>(this, (action) => ComReceived(action));
                    _loaded = true;
                    PatternCard = null;
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
                    Messenger.Default.Unregister<ComMessage>(this, (action) => ComReceived(action));
                    _loaded = false;
                    PatternCard = null;
                });
            }
        }

        ICommand _saveCardCommand;
        public ICommand SaveCardCommand
        {
            get
            {
                return _saveCardCommand ??= new RelayCommand(param => SaveCardToDb());
             }
        }

        ICommand _deleteCardCommand;
        public ICommand DeleteCardCommand
        {
            get
            {
                return _deleteCardCommand ??= new RelayCommand(param =>
                {
                    if (SelectedCard == null)
                        return;
                    Debug.WriteLine($"Удалим карту {SelectedCard.Card_Num}");
                    repository.Delete(SelectedCard.Card_Num);
                    Cards.Remove(SelectedCard);
                    Debug.WriteLine($"Удалим карту {SelectedCard.Card_Num}");

                    SelectedCard = null;
                });
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
                    SelectedCard.Status = int.Parse((string)param,CultureInfo.InvariantCulture);
                    Debug.WriteLine($"STATUS: { SelectedCard.Status}");
                    SaveCardToDb();                    
                });
            }
        }
    }
}
