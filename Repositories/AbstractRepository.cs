using System.Collections.Generic;
using Korobochka.GoogleSheets;
using Korobochka.Models;

namespace Korobochka.Repositories
{
    public abstract class AbstractRepository<T>
        : IRepository<T> where T : BaseModel, new()
    {
        private GoogleSheets.ICollectionClient<T> _client;

        public AbstractRepository(
            GoogleSheets.IClient client,
            GoogleSheets.ISettings settings,
            GoogleSheets.SheetSettings sheetSettings)
        {
            _client = new CollectionClient<T>(
                client: client,
                spreadsheetId: settings.Schema.SpreadsheetId,
                sheetSettings: sheetSettings
            );
        }

        public virtual IEnumerable<T> Get() => _client.Find();

        public virtual T Get(int id) => _client.Find(id);

        public virtual T Create(T item) => _client.InsertOne(item);

        public virtual T Update(int id, T itemIn) => _client.ReplaceOne(id, itemIn);

        public virtual void Remove(int id) => _client.DeleteOne(id);
    }
}
