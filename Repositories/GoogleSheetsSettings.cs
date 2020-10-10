namespace Korobochka.Repositories
{
    public class GoogleSheetsSettings : IGoogleSheetsSettings
    {
        public string SpreadsheetId { get; set; }
        public string PlacesRange { get; set; }
        public int PlacesSheetId { get; set; }
    }

    public interface IGoogleSheetsSettings
    {
         string SpreadsheetId { get; set; }
         string PlacesRange { get; set; }
         int PlacesSheetId { get; set; }
    }
}