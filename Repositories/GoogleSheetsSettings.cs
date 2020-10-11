namespace Korobochka.GoogleSheets
{
    public interface ISettings
    {
        string ApplicationName { get; set; }
        string Scopes { get; set; }
        string TokenPath { get; set; }

        string SpreadsheetId { get; set; }
        string PlacesRange { get; set; }
        int PlacesSheetId { get; set; }
    }
    
    public class Settings : ISettings
    {
        public string ApplicationName { get; set; }
        public string Scopes { get; set; }
        public string TokenPath { get; set; }

        public string SpreadsheetId { get; set; }
        public string PlacesRange { get; set; }
        public int PlacesSheetId { get; set; }
    }
}