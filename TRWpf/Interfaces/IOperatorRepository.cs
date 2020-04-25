using TRWpf.Models;

namespace TRWpf.Interfaces
{
    interface IOperatorRepository
    {
        Operator GetById(int id);
    }
}
