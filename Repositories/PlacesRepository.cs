using Korobochka.Models;

namespace Korobochka.Repositories
{
    public class PlacesRepository : AbstractRepository<Place>
    {
        public PlacesRepository(
            GoogleSheets.IClient driver,
            GoogleSheets.ISettings settings)
            : base(driver, settings, settings.PlacesRange, settings.PlacesSheetId)
        {
        }
    }
}