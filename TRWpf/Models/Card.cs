using System;
using System.Diagnostics;
using System.Globalization;
using TRWpf.Interfaces;
using TRWpf.ViewModels;

namespace TRWpf.Models
{
    public class Card : ViewModelBase, ICard
    {
        int? _user_id = null;
        int _status = 0;
        int _db_seg = 1;
        int _card_num = 0;
        DateTime _last_used = DateTime.MinValue;
        int _card_type = 0;
        public virtual User User { get; set; }

        public override string ToString()
        {
            return _card_num.ToString(new CultureInfo("en-us"));
        }

        public int? User_Id
        {
            get { return _user_id; }
            set
            {
                _user_id = value;
                OnPropertyChanged("User_Id");
            }
        }

        public int Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public int Db_Seg
        {
            get { return _db_seg; }
            set
            {
                _db_seg = value;
                OnPropertyChanged("Db_Seg");
            }
        }

        public int Card_Num
        {
            get { return _card_num; }
            set
            {
                _card_num = value;
                OnPropertyChanged("Card_Num");
            }
        }

        public DateTime Last_Used
        {
            get { return _last_used; }
            set
            {
                _last_used = value;
                OnPropertyChanged("Last_Used");
            }
        }

        public int Card_Type
        {
            get { return _card_type; }
            set
            { 
                _card_type = value;
                OnPropertyChanged("Card_Type");
            }
        }

        public void Log(string title)
        {
            if (title != null)
                Debug.WriteLine(title);
            Debug.WriteLine($"Card Num:  {Card_Num}");
            Debug.WriteLine($"Status:  {Status}");
            Debug.WriteLine($"User ID:  {User_Id}");
            Debug.WriteLine($"Last Used:  {Last_Used}");
            Debug.WriteLine($"Card Type:  {Card_Type}");
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged([CallerMemberName]string prop = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        //}
    }
}
