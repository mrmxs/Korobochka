using Korobochka.Models;

namespace Korobochka.Repositories
{
    public class PlacesRepository : AbstractRepository<Place>
    {
        public PlacesRepository(IGoogleSheetsSettings settings)
            : base(settings, settings.PlacesRange, settings.PlacesSheetId)
        {
        }
    }
}