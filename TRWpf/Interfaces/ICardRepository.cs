using System.Collections.ObjectModel;
using TRWpf.Models;

namespace TRWpf.Interfaces
{

    interface ICardRepository
    {
        void Delete(int id);
        Card GetById(int id);
        // void Create(Card card);
        // void Update(Card card);
        void UpdateOrInsert(Card card);
    }
}
