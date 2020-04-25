using TRWpf.Interfaces;
using TRWpf.ViewModels;

namespace TRWpf.Models
{
    public class Acc_Level : ViewModelBase, IAcc_Level
    {
        int _acc_lvl = 0;
        string _acl_name = "";

        public int Acc_Lvl
        {
            get { return _acc_lvl; }
            set
            {
                _acc_lvl = value;
                OnPropertyChanged("Acc_Lvl");
            }
        }

        public string Acl_Name
        {
            get { return _acl_name; }
            set
            {
                _acl_name = value;
                OnPropertyChanged("Acl_Name");
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged([CallerMemberName]string prop = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        //}
    }
}
