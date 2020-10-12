using Korobochka.DTOs;
using Korobochka.Models;
using Korobochka.Repositories;

namespace Korobochka.Services
{
    public class PlacesCRUDService : AbstractCRUDService<Place, PlaceDTO>
    {
        public PlacesCRUDService(PlacesRepository repository)
            : base(repository)
        {
        }

        protected override Place Convert(PlaceDTO itemIn)
        {
            return (Place)itemIn;
        }
    }
}