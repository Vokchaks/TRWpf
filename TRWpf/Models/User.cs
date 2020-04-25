using System;
using System.Diagnostics;
using TRWpf.DataAccess;
using TRWpf.Interfaces;
using TRWpf.ViewModels;

namespace TRWpf.Models
{
    public class User : ViewModelBase, IUser
    {
        int _user_id = 0;
        string _name = "";
        string _family = "";
        string _fname = "";
        string _full_name = "";
        int _group = 1;
        string _position = "";
        int _dept_id = 1;
        DateTime _activate;
        DateTime _deactivate;
        int _acc_lvl = 0;

        public DateTime Deactivate
        {
            get { return _deactivate; }
            set
            {
                _deactivate = value;
                OnPropertyChanged("Deactivate");
            }
        }

        public DateTime Activate
        {
            get { return _activate; }
            set
            {
                _activate = value;
                OnPropertyChanged("Activate");
            }
        }

        public int Acc_Lvl
        {
            get { return _acc_lvl; }
            set
            {
                _acc_lvl = value;
                OnPropertyChanged("Acc_Lvl");
            }
        }

        public int Dept_Id
        {
            get { return _dept_id; }
            set
            {
                _dept_id = value;
                OnPropertyChanged("Dept_Id");
            }
        }

        public override string ToString()
        {
            return Full_Name;
        }

        public string Position
        {
            get {  return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        public int Group
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged("Group");
            }
        }

        public string Full_Name
        {
            get { return _full_name; }
            set 
            {
                _full_name = value;
   //             OnPropertyChanged("Full_Name");
            }
        }

        
        public int User_Id
        {
            get { return _user_id; }
            set
            {
                _user_id = value;
                OnPropertyChanged("User_Id");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
                _full_name = _family + " " + _name + " " + _fname;
                OnPropertyChanged("Full_Name");
            }
        }

        public string Fname
        {
            get { return _fname; }
            set
            {
                _fname = value;
                OnPropertyChanged("Fname");
                _full_name = _family + " " + _name + " " + _fname;
                OnPropertyChanged("Full_Name");
            }
        }
        public string Family
        {
            get { return _family; }
            set
            {
                _family = value;
                OnPropertyChanged("Family");
                _full_name = _family + " " + _name + " " + _fname;
                OnPropertyChanged("Full_Name");
            }
        }

        public void Log(string title)
        {
            if(title != null)
                Debug.WriteLine(title);
            Debug.WriteLine($"FULL NAME:  {Full_Name}");
            Debug.WriteLine($"ID:  {User_Id}");
            Debug.WriteLine($"Name:  {Name}");
            Debug.WriteLine($"Fname:  {Fname}");
            Debug.WriteLine($"Family:  {Family}");

            string level = Acc_LevelRepository.GetById(Acc_Lvl)?.Acl_Name;
            Debug.WriteLine($"Acc_Lvl:  {Acc_Lvl}  {level}");
            Debug.WriteLine($"Activate:  {Activate}");
            Debug.WriteLine($"Deactivate:  {Deactivate}");
            Debug.WriteLine($"Group:  {Group}");
            Debug.WriteLine($"Position:  {Position}");
            Debug.WriteLine($"Dept_id:  {Dept_Id}");
        }
    }
}
