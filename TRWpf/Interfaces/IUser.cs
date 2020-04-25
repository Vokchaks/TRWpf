
using System;

namespace TRWpf.Interfaces
{
    interface IUser
    {
        int User_Id {get;set;}
        string Name { get; set; }
        string Fname { get; set; }
        string Family { get; set; }
        int Group { get; set; }
        string Position { get; set; }
        int Dept_Id { get; set; }
        int Acc_Lvl  { get; set; }
        DateTime Activate { get; set; }
        DateTime Deactivate { get; set; }
    }
}
