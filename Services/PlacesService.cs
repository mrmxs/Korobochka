using Korobochka.Models;
using Korobochka.Repositories;

namespace Korobochka.Services
{
    public class PlacesService : AbstractCRUDService<Place>
    {
        public PlacesService(PlacesRepository repository)
            : base(repository)
        {
        }
    }
}