namespace Korobochka.GoogleSheets
{
    public interface ISettings
    {
        string ApplicationName { get; set; }
        string Scopes { get; set; }
        string TokenPath { get; set; }
        SpreadsheetSchema Schema { get; set; }
    }

    public class Settings : ISettings
    {
        public string ApplicationName { get; set; }
        public string Scopes { get; set; }
        public string TokenPath { get; set; }
        public SpreadsheetSchema Schema { get; set; }
    }

    public class SpreadsheetSchema
    {
        public string SpreadsheetId { get; set; }
        public SheetSettings PlacesSheet { get; set; }
    }

    public class SheetSettings
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Range { get; set; }
    }
}
