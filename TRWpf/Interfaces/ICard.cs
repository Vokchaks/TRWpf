using System;

namespace TRWpf.Interfaces
{
    interface ICard
    {
        int Card_Num { get; set; }
        int Card_Type { get; set; }
        int? User_Id { get; set; }
        int Status { get; set; }
        int Db_Seg { get; set; }
        DateTime Last_Used { get; set; }
    }
}
