using TRWpf.Models;

namespace TRWpf.Interfaces
{
    interface IUser_PhotoRepository
    {
        void Delete(int id);
        User_Photo GetById(int id);
        void UpdateOrInsert(User_Photo up);
    }
}

