using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;

namespace Korobochka.GoogleSheets
{
    public class Client : IClient
    {
        #region CONSTs
        const string TOKEN_PATH = "token.json/client_secret.json";
        private readonly Dictionary<string, string> SCOPES = new Dictionary<string, string> {
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
        #endregion
    }
}