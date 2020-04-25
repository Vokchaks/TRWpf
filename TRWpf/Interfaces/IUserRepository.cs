using TRWpf.Models;

namespace TRWpf.Interfaces
{
    interface IUserRepository
    {
        int Create(User user);
        void Delete(int id);
        User GetById(int id);
        void Update(User user);
    }
}
