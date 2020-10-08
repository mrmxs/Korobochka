using System.Collections.Generic;
using Korobochka.Models;
using Korobochka.Repositories;

namespace Korobochka.Services
{
    public abstract class AbstractCRUDService<T>
        : ICRUDService<T> where T : BaseModel
    {
        private readonly IRepository<T> _repository;

        public AbstractCRUDService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual IEnumerable<T> Get() => _repository.Get();

        public virtual T Get(int id) => _repository.Get(id);

        public virtual T Create(T item) => _repository.Create(item);

        public virtual T Update(int id, T item) => _repository.Update(id,  item);

        public virtual void Remove(T item) => _repository.Remove(item);

        public virtual void Remove(int id) => _repository.Remove(id);
    }
}
