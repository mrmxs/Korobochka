using System.Collections.Generic;
using Korobochka.Models;

namespace Korobochka.Repositories
{
    public interface IRepository<T> where T : BaseModel
    {
        IEnumerable<T> Get();
        T Get(int id);
        T Create(T item);
        T Update(int id, T itemIn);
        void Remove(int id);
    }
}