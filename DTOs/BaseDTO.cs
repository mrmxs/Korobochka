namespace Korobochka.DTOs
{
    public abstract class BaseDTO
    {
        public int Id { get; set; }
        public string GSheetRange { get; internal set; }
    }
}

