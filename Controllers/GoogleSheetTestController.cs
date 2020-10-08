using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using Korobochka.Models;
using Korobochka.Repositories; // TODO replace using of googleSheetsSettings to repositories
using Korobochka.Services;

namespace Korobochka.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleSheetTestController : ControllerBase
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string _applicationName = "Korobochka";
        static string[] _scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        // static readonly string[] _scopeRequiredByApi = new string[] { "access_as_user" };
        private readonly IGoogleSheetsSettings _gSheetsSettings;

        private readonly ILogger<GoogleSheetTestController> _logger;

        private PlacesService _placesService;

        public GoogleSheetTestController(
            IGoogleSheetsSettings gSheetsSettings,
            ILogger<GoogleSheetTestController> logger,
            PlacesService placesService)
        {
            _logger = logger;
            _gSheetsSettings = gSheetsSettings;
            _placesService = placesService;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {

            UserCredential credential;

            using (var stream =
                new FileStream("token.json/client_secret.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });

            // Define request parameters.
            // String spreadsheetId = "1Fd09448owbmW9DLGbcd9qC_KUoyGux3zHFHoAps9WS0"; // <-- Korobochka
            // String range = "Places!A2:G";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    // service.Spreadsheets.Values.Get(spreadsheetId, range);
                    service.Spreadsheets.Values.Get(
                        _gSheetsSettings.SpreadsheetId, _gSheetsSettings.PlacesRange);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1Fd09448owbmW9DLGbcd9qC_KUoyGux3zHFHoAps9WS0/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                var result = new List<string>();
                foreach (var row in values)
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    // Console.WriteLine("{0}\t{1}", row[1], row[1]);
                    result.Add($"#{row[0]}({row[6]})\t{row[1]}");
                }
                return result;
            }
            return new string[] { "No ", "data ", "found." };

        }

        [HttpPut]
        public IEnumerable<Place> Put()
        {
            return _placesService.Get();
        }
    }
}
