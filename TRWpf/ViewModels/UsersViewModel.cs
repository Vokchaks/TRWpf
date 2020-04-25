using System.Collections.ObjectModel;
using TRWpf.Models;
using TRWpf.DataAccess;
using TRWpf.Views;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Data;
using System.ComponentModel;
using System;
using GalaSoft.MvvmLight.Messaging;

namespace TRWpf.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        bool listLoaded = false;
        public ObservableCollection<Card> Cards { get; set; }

        private bool _createButton;
        public bool CreateButton
        {
            get { return _createButton; }
            set
            {
                _createButton = value;
                OnPropertyChanged("CreateButton");
            }
        }

        ICommand _createUserCommand;
        public ICommand CreateUserCommand
        {
            get
            {
                return _createUserCommand ?? (_createUserCommand = new RelayCommand(param => CreateUser()));
            }
        }

        private void CreateUser()
        {
            DateTime date = DateTime.Now;
            User user = new User
            {
                User_Id = 0,
                Group = SelectedGroup,
                Dept_Id = 1,
                Activate = date,
                Deactivate = date.AddDays(1),
                Position = ""
            };

            EditUser(user);
            user?.Log("ADD USER AFTER CREATE");

            if (user.User_Id > 0)            
                Users.Add(user);
        }

        public IEnumerable<Element> UserGroupViewModels { get; private set; }

        User _selectedUser = null;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (!listLoaded)
                    return;
                _selectedUser = value;
                OnPropertyChanged("SelectedUser");
#if DEBUG
                _selectedUser?.Log("SELECTED USER");
#endif
            }
        }

        ICommand _showMessageCommand;
        public ICommand ShowMessageCommand
        {
            get
            {
                return _showMessageCommand ??= new RelayCommand(o => { MessageBox.Show("Получилось"); });
            }
        }

        private void EditUser(object user)
        {
            if (!(user is User u))
                return;

            Debug.WriteLine($"EDIT USER ID: {u.User_Id}");

            var uvm = new UserViewModel
            {
                user = u,
                Cards = Cards
            };
            uvm.InitUser();

            UserWindow uw = new UserWindow
            {
                DataContext = uvm
            };
            uw.ShowDialog();
        }

        ICommand _editUserCommand;
        public ICommand EditUserCommand
        {
            get
            {
                return _editUserCommand ??= new RelayCommand(obj => EditUser(obj));
            }
        }

        ICommand _deleteUserCommand;
        public ICommand DeleteUserCommand
        {
            get
            {
                return _deleteUserCommand ??= new RelayCommand(obj => DeleteUser());
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                Debug.WriteLine($"Delete User {SelectedUser.Full_Name}");
                DeletePhoto(SelectedUser.User_Id);
                CleanCards(SelectedUser.User_Id);

                UserRepository ur = new UserRepository();
                ur.Delete(SelectedUser.User_Id);
                Users.Remove(SelectedUser);                
            }
            SelectedUser = null;
        }

        private void CleanCards(int id)
        {
            if (id < 1)
                return;
            ObservableCollection<Card> userCards = new ObservableCollection<Card>(Cards.Where(c => c.User_Id == id).Select(c => c));
            if (userCards == null || userCards.Count < 1)
                return;
            CardRepository cr = new CardRepository();
            foreach (var item in userCards)
            {
                item.User_Id = null;
                item.Status = 0;
                item.Card_Type = 0;
                cr.UpdateOrInsert(item);
            }
        }

        private static void DeletePhoto(int id)
        {
            User_PhotoRepository upr = new User_PhotoRepository();
            if (upr.GetById(id) != null)
                upr.Delete(id);
        }


        ICollectionView _usersView;
        public ICollectionView UsersView
        {
            get { return _usersView; }
            set
            {
               // Debug.WriteLine($"UserView");
                _usersView = value;
                OnPropertyChanged("UserView");
            }
        }



        private int _selectedGroup;
        public int SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                if (_selectedGroup > 1)
                    CreateButton = true;
                else
                    CreateButton = false;
                UsersView.Refresh();
                OnPropertyChanged("SelectedGroup");
            }
        }

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                UsersView = CollectionViewSource.GetDefaultView(Users);
                OnPropertyChanged("User");
                Debug.WriteLine($"Users changed: {_users.Count}");
                Messenger.Default.Send<StatusMessage>(
                    new StatusMessage
                    {
                        Msg = String.Intern("Зарегистрировано пользователей: ") + Users.Count
                    });
            }
        }

        ICommand _listLoadedCommand;
        public ICommand ListLoadedCommand
        {
            get
            {
                return _listLoadedCommand ??= new RelayCommand(param =>
                {
                    listLoaded = true;
                });
            }
        }


        ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                return _loadedCommand ??= new RelayCommand(param =>
                {
                    Messenger.Default.Send<StatusMessage>(
                        new StatusMessage
                        {
                            Msg = String.Intern("Зарегистрировано пользователей: ") + Users.Count
                        });
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
                    Debug.WriteLine("USER UN-LOADED");
                });
            }
        }

        public UsersViewModel(ObservableCollection<User> users)
        {
            CreateButton = false;
            Users = users;

            UserGroupViewModels = Element.UserGroupViewModels;
            UsersView.SortDescriptions.Add(new SortDescription("Full_Name", ListSortDirection.Ascending));
            UsersView.Filter += UsersFilter;
        }

        public bool UsersFilter(object obj)
        {
            if (SelectedGroup == 0)
                return true;

            if (obj is User user)
            {
                return user.Group == SelectedGroup;
            }
            return false;
        }

        StringComparison comparison = StringComparison.OrdinalIgnoreCase;
        string _patternUser = null;
        public string PatternUser
        {
            get { return _patternUser; }
            set
            {
                if (_patternUser == null ) return;
                // Debug.WriteLine($"Pattern: {value}");
                _patternUser = value;
                SelectedUser = Users.FirstOrDefault(u => u.Full_Name.StartsWith(_patternUser, comparison));
                OnPropertyChanged("PatternUser");
            }
        }
    }
}
