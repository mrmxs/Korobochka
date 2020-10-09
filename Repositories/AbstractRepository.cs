using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GSheets = Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;
using Korobochka.Models;

namespace Korobochka.Repositories
{
    public abstract class AbstractRepository<T>
        : IRepository<T> where T : BaseModel, new()
    {
        private readonly SheetsService _service;
        private readonly string _spreadsheetId;
        private readonly string _sheetRange;

        public AbstractRepository(IGoogleSheetsSettings settings, string sheetRange) //TODO mb sheetRange is obsolete
        {
            UserCredential credential;

            // The file token.json stores the user's access and refresh tokens, and is created
            // automatically when the authorization flow completes for the first time.
            const string TokenFolderPath = "token.json";
            const string TokenPath = "token.json/client_secret.json"; //TODO move to settings
            // If modifying these scopes, delete your previously saved credentials
            // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
            // string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly };
            string[] scopes = { SheetsService.Scope.Spreadsheets };
            using (var stream =
                new FileStream(TokenPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(TokenFolderPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + TokenFolderPath);
            }

            var _applicationName = "Korobochka"; //TODO move to settings
            // Create Google Sheets API service.
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });

            _spreadsheetId = settings.SpreadsheetId;
            _sheetRange = sheetRange;
        }

        protected IList<IList<object>> GSheetCollection() //TODO to external file GSheetDriver
        {
            return _service.Spreadsheets.Values
                .Get(_spreadsheetId, _sheetRange)
                .Execute()
                .Values;
        }

        protected IList<IList<object>> GSheetGetRange(string range) //TODO to external file GSheetDriver
        {
            return _service.Spreadsheets.Values
                .Get(_spreadsheetId, range)
                .Execute()
                .Values;
        }

        protected UpdateValuesResponse GSheetAppend(List<IList<object>> values)
        {
            ValueRange valueRange = new ValueRange();
            valueRange.Values = values;
            var range = _sheetRange.Split('!')[0];

            GSheets.AppendRequest request =
                _service.Spreadsheets.Values
                .Append(valueRange, _spreadsheetId, range);
            request.ValueInputOption = GSheets.AppendRequest.ValueInputOptionEnum.RAW;
            AppendValuesResponse response = request.Execute();

            return response.Updates;
        }

        protected int GSheetMaxId()
        {
            //TODO get MAX(A:A) from sheets or GetByDataFilter()
            return new T().FromValues<T>(
                this.GSheetCollection().Last()).Id;
        }

        public virtual IEnumerable<T> Get()
        {
            var result = this.GSheetCollection();

            var res = result?
                .Where(row => row.Any())?
                .Select(row => new T().FromValues<T>(row));

            return res ?? new List<T>();
        }

        public virtual T Get(int id) =>
            this.Get().ToList().Find(item => item?.Id == id);

        public virtual T Create(T item)
        {
            item.Id = this.GSheetMaxId() + 1;
            // TODO order

            var appended = this.GSheetAppend(
                values: new List<IList<object>> { item.ToValues<T>(item) });
            var appendedValues = this.GSheetGetRange(appended.UpdatedRange).First();

            var result = new T().FromValues<T>(appendedValues);
            result.GSheetRange = appended.UpdatedRange;

            return result;
        }

        public virtual T Update(int id, T itemIn)
        {
            throw new NotImplementedException();

            // var oldItem = this.Get(id);
            // var newItem = oldItem.Merge(itemIn);

            // if (oldItem.MemberwiseEquals(newItem)) return oldItem;

            // newItem.EditedAt = DateTime.Now;
            // _collection.ReplaceOne(item => item.Id == id, newItem);

            // return this.Get(id);
        }

        // public virtual void Remove(T itemIn) =>
        //     _collection.DeleteOne(item => item.Id == itemIn.Id);
        public virtual void Remove(T itemIn)
        {
            throw new NotImplementedException();
        }

        // public virtual void Remove(int id) =>
        //     _collection.DeleteOne(item => item.Id == id);
        public virtual void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
