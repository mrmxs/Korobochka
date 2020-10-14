using System;
using System.Collections.Generic;
using System.Linq;
using Korobochka.Models;

namespace Korobochka.GoogleSheets
{
    public class CollectionClient<T> : ICollectionClient<T>
        where T : BaseModel, new()
    {
        private GoogleSheets.IClient _client;
        private readonly string _spreadsheetId;
        private readonly SheetSettings _sheetSettings;

        public CollectionClient(
            GoogleSheets.IClient client,
            string spreadsheetId,
            SheetSettings sheetSettings)
        {
            _spreadsheetId = spreadsheetId;
            _sheetSettings = sheetSettings;
            _client = client;
        }

        public IEnumerable<T> Find()
        {
            var result = _client.Get(
                _spreadsheetId, _sheetSettings.Title, _sheetSettings.Range);

            var res = result?
                .Where(row => row.Any())?
                .Select(row => new T().FromValues<T>(row));

            return res ?? new List<T>();
        }

        public T Find(int id)
        {
            var rowIndex = this.GetRowIndexByID(id);
            if (null == rowIndex) return null;

            var values = _client.GetRow(
                _spreadsheetId,
                _sheetSettings.Title,
                _sheetSettings.Range,
                rowIndex.Value);

            var result = new T().FromValues<T>(values);
            result.GSheetRange = string.Join('!', new string[] {
                _sheetSettings.Title,
                _sheetSettings.Range.Replace("A2", $"A{rowIndex}")
            });

            return result;
        }

        public T InsertOne(T item)
        {
            item.Id = this.GetMaxID();
            // TODO order
            var appended = _client.InsertOne(
                _spreadsheetId, _sheetSettings.Title, item.ToValues<T>());

            var appendedValues = _client
                .GetByFullRange(_spreadsheetId, appended.UpdatedRange)
                .First();
            var result = new T().FromValues<T>(appendedValues);
            result.GSheetRange = appended.UpdatedRange;

            return result;
        }

        public T ReplaceOne(int id, T itemIn)
        {
            var oldItem = this.Find(id);
            if (null == oldItem) throw new Exception($"Not existing id");

            var newItem = oldItem.Merge(itemIn);
            if (oldItem.MemberwiseEquals(newItem)) return oldItem;

            // TODO place for itemHistory is here

            var response = _client.ReplaceOne(
                _spreadsheetId,
                _sheetSettings.Title,
                oldItem.GSheetRange,
                newItem.ToValues<T>());

            var updatedValues = _client
                .GetByFullRange(_spreadsheetId, response.UpdatedRange)
                .First();
            var updatedData = new T().FromValues<T>(updatedValues);
            updatedData.GSheetRange = response.UpdatedRange;

            return updatedData;
        }

        public void DeleteOne(int id)
        {
            var rowIndex = this.GetRowIndexByID(id);
            if (null == rowIndex) throw new Exception($"Not existing id");

            _client.DeleteOne(_spreadsheetId, _sheetSettings.Id, rowIndex.Value);
        }

        public int GetMaxID() => _client.GetMaxInColumn(
            _spreadsheetId, _sheetSettings.Title, column: "A");


        public int? GetRowIndexByID(int id)
        {
            var ids = _client.GetColumn(
                _spreadsheetId, _sheetSettings.Title, column: "A");
            var index = ids?
                .Select(row => int.Parse(row[0].ToString()))
                .ToList()
                .IndexOf(id) + 2;

            return 1 < index ? index : null;
        }
    }
}
