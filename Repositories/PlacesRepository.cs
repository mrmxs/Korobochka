using Korobochka.Models;

namespace Korobochka.Repositories
{
    public class PlacesRepository : AbstractRepository<Place>
    {
        public PlacesRepository(
            GoogleSheets.IClient client,
            GoogleSheets.ISettings settings)
            : base(client, settings, settings.Schema.PlacesSheet,
                $"{settings.Schema.PlacesSheet.Title}!{settings.Schema.PlacesSheet.Range}",
                settings.Schema.PlacesSheet.Id)
        {
        }
    }
}