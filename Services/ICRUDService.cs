using System.Collections.Generic;
using Korobochka.DTOs;
using Korobochka.Models;

namespace Korobochka.Services
{
    public interface ICRUDService<T,D> where T : BaseModel where D : BaseDTO
    {
        IEnumerable<T> Get();
        T Get(int id);
        T Create(D itemIn);
        T Update(int id, D itemIn);
        void Remove(int id);
    }
}