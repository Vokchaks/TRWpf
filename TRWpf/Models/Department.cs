using TRWpf.ViewModels;

namespace TRWpf.Models
{
    public class Department : ViewModelBase
    {
        private int _dept_id = 0;
        private string _name = "";
       
        public int Dept_Id
        {
            get { return _dept_id; }
            set
            {
                _dept_id = value;
                OnPropertyChanged("Dept_Id");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged([CallerMemberName]string prop = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        //}
    }
}
