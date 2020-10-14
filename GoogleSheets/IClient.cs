using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Korobochka.Models;

namespace Korobochka.GoogleSheets
{
    public interface IClient
    {
        UserCredential Credential { get; }
        SheetsService Service { get; }
        IList<IList<object>> Get(string spreadsheetId, string sheetTitle, string range);
        IList<IList<object>> GetByFullRange(string spreadsheetId, string fullRange);
        IList<IList<object>> GetColumn(string spreadsheetId, string sheetTitle, string column);
        IList<object> GetRow(string spreadsheetId, string sheetTitle, string headingRange, int rowIndex);
        UpdateValuesResponse InsertOne(string spreadsheetId, string sheetTitle, IList<object> values);
        UpdateValuesResponse ReplaceOne(string spreadsheetId, string sheetTitle, string range, IList<object> values);
        void DeleteOne(string spreadsheetId, int sheetId, int rowIndex);

        int GetMaxInColumn(string spreadsheetId, string sheetTitle, string column);
        int? GetRowIndexByCellText(string spreadsheetId, string sheetTitle, string column, string cellText);
    }

    public interface ICollectionClient<T> where T : BaseModel
    {
        IEnumerable<T> Find();
        T Find(int id);
        T InsertOne(T item);
        T ReplaceOne(int id, T itemIn);
        void DeleteOne(int id);

        int GetMaxID();
        int? GetRowIndexByID(int id);
    }
}
