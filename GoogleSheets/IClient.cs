using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;

namespace Korobochka.GoogleSheets
{
    public interface IClient
    {
        UserCredential Credential { get; }
        SheetsService Service { get; }
    }

    public interface ICollectionClient
    {
        IList<IList<object>> Find(); //item => true
        IList<object> Find(int id);
        IList<object> InsertOne(IList<object> item);
        IList<object> ReplaceOne(int id, IList<object> item);
        void DeleteOne(int id);
    }
}