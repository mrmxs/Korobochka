using Korobochka.Models;
using Korobochka.Repositories;

namespace Korobochka.Services
{
    public class PlacesCRUDService : AbstractCRUDService<Place>
    {
        public PlacesCRUDService(PlacesRepository repository)
            : base(repository)
        {
        }
    }
}