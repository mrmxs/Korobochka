using System.Collections.Generic;
using Korobochka.Models;

namespace Korobochka.Services
{
    public interface ICRUDService<T> where T : BaseModel
    {
        IEnumerable<T> Get();
        T Get(int id);
        T Create(T item);
        T Update(int id, T itemIn);
        void Remove(T itemIn);
        void Remove(int id);
    }
}