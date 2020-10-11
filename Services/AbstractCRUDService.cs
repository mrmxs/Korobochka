using System.Collections.Generic;
using Korobochka.DTOs;
using Korobochka.Models;
using Korobochka.Repositories;

namespace Korobochka.Services
{
    public abstract class AbstractCRUDService<T,D>
        : ICRUDService<T,D> where T : BaseModel where D : BaseDTO
    {
        private readonly IRepository<T> _repository;

        public AbstractCRUDService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual IEnumerable<T> Get() => _repository.Get();

        public virtual T Get(int id) => _repository.Get(id);

        public virtual T Create(D itemIn) => _repository.Create(Convert(itemIn));

        public virtual T Update(int id, D itemIn) => _repository.Update(id, Convert(itemIn));

        public virtual void Remove(int id) => _repository.Remove(id);

        protected abstract T Convert(D itemIn);
    }
}
