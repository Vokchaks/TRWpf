using TRWpf.ViewModels;

namespace TRWpf.Models
{
    public class User_Photo : ViewModelBase
    {
        int _user_id = 0;
        byte[] _photo = null;

        public bool Change { get; set; } = false;
        public int User_Id
        {
            get { return _user_id; }
            set
            {
                _user_id = value;
                OnPropertyChanged("User_Id");
            }
        }

        public byte[] Photo
        {
            get { return _photo; }
            set
            {
                if(_photo != null)
                    Change = true;
                _photo = value;
                OnPropertyChanged("Photo");
            }
        }
    }
}
