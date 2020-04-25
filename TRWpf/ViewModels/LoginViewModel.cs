using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TRWpf.Views;
using TRWpf.DataAccess;

namespace TRWpf.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        OperatorRepository or;
        
        ICommand _submitCommand;
        public ICommand SubmitCommand
        {
            get
            {
                return _submitCommand ?? (_submitCommand = new RelayCommand(param =>
                {
                    var values = (object[])param;
                    PasswordBox password = (PasswordBox)values[0];
                    string _password = password.Password;

                    if (Login.Length > 0 &&  !String.IsNullOrEmpty (_password) && _password.Length > 0)
                    {
                        if (or.GetByLoginPassword(Login, _password) != null)
                        {
                            LoginSuccess = false;
                          
                            MainWindow mw = new MainWindow();
                            mw.Show();

                            Window login = (Window)values[1];
                            login.Close(); 
                        }
                    }
                    Login = "";
                    password.Password = "";
                }));
            }
        }

        ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommand(param =>
                {
                    if (param is Window window)
                        window.Close();
                }));
            }
        }

        private bool _loginsuccess = true;
        public bool LoginSuccess
        {
            get { return _loginsuccess; }
            set
            {
                _loginsuccess = value;
                OnPropertyChanged("LoginSuccess");
            }
        }

        private string _login = "";
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }

        public LoginViewModel()
        {
            or = new OperatorRepository();
            if (!or.TestConnection())
            {
                MessageBox.Show("Ошибка доступа к базе.", String.Intern("Критическа ошибка"), MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
    }
}
