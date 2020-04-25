using TRWpf.Interfaces;
using TRWpf.ViewModels;

namespace TRWpf.Models
{
    public class Operator : ViewModelBase, IOperator
    {
        private int _op_id = 0;
        private string _login = "";
        private string _password = "";
        private string _full_name = "";

        public int Op_Id {
            get  { return _op_id; }
            set
            {
                _op_id = value;
                OnPropertyChanged("Op_Id");
            }
        }
        public string Login
        {
            get
            { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        public string Full_Name
        {
            get { return _full_name; }
            set
            {
                _full_name = value;
                OnPropertyChanged("Full_Name");
            }
        }
    }
}
