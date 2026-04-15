namespace CarService.API.Models.DTOs
{
    public class PartDto
    {
        public string  Name          { get; set; } = string.Empty;
        public string  ArticleNumber { get; set; } = string.Empty;
        public decimal Price         { get; set; }
        public int     Quantity      { get; set; }
        public string? Category      { get; set; }
    }
}
