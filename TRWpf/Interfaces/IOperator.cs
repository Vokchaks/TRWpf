using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shell;

namespace TRWpf.Interfaces
{
    interface IOperator
    {
        int Op_Id { get; set; }
        string Login { get; set; }
        string Password { get; set; }
        string Full_Name { get; set; }
    }
}
