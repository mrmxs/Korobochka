using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;

namespace Korobochka.GoogleSheets
{
    public interface IDriver
    {
        UserCredential Credential { get; }
        SheetsService Service { get; }
    }
}