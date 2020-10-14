using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace Korobochka.GoogleSheets
{
    public class Client : IClient
    {
        #region CONSTs
        const string TOKEN_PATH = "token.json/client_secret.json";
        private readonly Dictionary<string, string> SCOPES =
            new Dictionary<string, string> {
                { "Spreadsheets", SheetsService.Scope.Spreadsheets },
                { "SpreadsheetsReadonly", SheetsService.Scope.SpreadsheetsReadonly },
            };
        #endregion

        private readonly ISettings _settings;

        public Client(ISettings settings)
        {
            _settings = settings;
            this.SetCredential();
            this.SetService(Credential);
        }

        #region Private
        private void SetCredential()
        {
            string tokenPath = string.IsNullOrEmpty(_settings.TokenPath)
                ? TOKEN_PATH
                : _settings.TokenPath;
            string folder = tokenPath.Contains('/')
                ? tokenPath.Split('/')[0]
                : TOKEN_PATH.Split('/')[0];
            string[] scopes = _settings.Scopes
                .Split(',').Select(scope => SCOPES[scope.Trim()]).ToArray();

            using (var stream =
                new FileStream(tokenPath, FileMode.Open, FileAccess.Read))
            {
                Credential = GoogleWebAuthorizationBroker
                    .AuthorizeAsync(
                        clientSecrets: GoogleClientSecrets.Load(stream).Secrets,
                        scopes: scopes,
                        user: "user",
                        taskCancellationToken: CancellationToken.None,
                        dataStore: new FileDataStore(folder, fullPath: true)
                    ).Result;

                // TODO logger
                // Console.WriteLine("Credential file saved to: " + TokenFolderPath);
            }
        }

        private void SetService(UserCredential credential)
        {
            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _settings.ApplicationName,
            });
        }
        #endregion

        #region Public
        public UserCredential Credential { get; private set; }
        public SheetsService Service { get; private set; }

        public IList<IList<object>> Get(
            string spreadsheetId, string sheetTitle, string range)
        {
            return Service.Spreadsheets.Values
               .Get(spreadsheetId, $"{sheetTitle}!{range}")
               .Execute()
               .Values;
        }

        public IList<IList<object>> GetByFullRange(
            string spreadsheetId, string fullRange)
        {
            return Service.Spreadsheets.Values
               .Get(spreadsheetId, fullRange)
               .Execute()
               .Values;
        }

        public IList<IList<object>> GetColumn(
            string spreadsheetId, string sheetTitle, string column)
        {
            return Service.Spreadsheets.Values.Get(
                    spreadsheetId,
                    $"{sheetTitle}!{column}2:{column}"
                ).Execute().Values;
        }

        public IList<object> GetRow(
            string spreadsheetId,
            string sheetTitle,
            string headingRange,
            int rowIndex)
        {
            var fullRange = string.Join('!', new string[] {
                sheetTitle,
                Regex.Replace(headingRange,
                    @"(?<A>[^\d]+)(\d+)", "${A}" + rowIndex)
            });

            var result = this.GetByFullRange(spreadsheetId, fullRange).First();
            result.Add(rowIndex);

            return result;
        }

        public UpdateValuesResponse InsertOne(
            string spreadsheetId, string sheetTitle, IList<object> values)
        {
            ValueRange valueRange = new ValueRange();
            valueRange.MajorDimension = "ROWS";
            valueRange.Values = new List<IList<object>> { values };

            AppendRequest request =
                Service.Spreadsheets.Values
                .Append(valueRange, spreadsheetId, sheetTitle);
            request.ValueInputOption = AppendRequest.ValueInputOptionEnum.RAW;
            AppendValuesResponse response = request.Execute();

            return response.Updates;
        }

        public UpdateValuesResponse ReplaceOne(
            string spreadsheetId,
            string sheetTitle,
            string range,
            IList<object> values)
        {
            ValueRange valueRange = new ValueRange();
            valueRange.MajorDimension = "ROWS";
            valueRange.Values = new List<IList<object>> { values };

            UpdateRequest update =
                Service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            update.ValueInputOption = UpdateRequest.ValueInputOptionEnum.RAW;
            UpdateValuesResponse response = update.Execute();

            return response;
        }

        public void DeleteOne(
            string spreadsheetId, int sheetId, int rowIndex)
        {
            Request requestBody = new Request()
            {
                DeleteDimension = new DeleteDimensionRequest()
                {
                    Range = new DimensionRange()
                    {
                        SheetId = sheetId,
                        Dimension = "ROWS",
                        StartIndex = rowIndex - 1,
                        EndIndex = rowIndex,
                    }
                }
            };

            BatchUpdateSpreadsheetRequest request = 
                new BatchUpdateSpreadsheetRequest();
            request.Requests = new List<Request> { requestBody };
            BatchUpdateSpreadsheetResponse response = new SpreadsheetsResource
                .BatchUpdateRequest(Service, request, spreadsheetId)
                .Execute();
        }

        public int? GetRowIndexByCellText(
            string spreadsheetId,
            string sheetTitle,
            string column,
            string cellText)
        {
            var cells = this.GetColumn(spreadsheetId, sheetTitle, column);
            var index = cells?
                .Select(row => row[0].ToString()).ToList()
                .IndexOf(cellText) + 2;

            return index;
        }

        public int GetMaxInColumn(
            string spreadsheetId, string sheetTitle, string column)
        {
            var ids = this.GetColumn(spreadsheetId, sheetTitle, column);
            if (ids == null) return 0;
            var result = ids!
                .Select(row => int.Parse(row[0].ToString()))
                .Max();

            return result;
        }
        #endregion
    }
}
