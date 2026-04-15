// ===== CarDto.cs =====
namespace CarService.API.Models.DTOs
{
    public class CarDto
    {
        public string LicensePlate { get; set; } = string.Empty;
        public string Brand        { get; set; } = string.Empty;
        public string Model        { get; set; } = string.Empty;
    }
}
