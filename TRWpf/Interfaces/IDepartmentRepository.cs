using TRWpf.Models;

namespace TRWpf.Interfaces
{
    interface IDepartmentRepository
    {
        Department GetById(int id);
    }
}
