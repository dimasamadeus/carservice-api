using avtoservis;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarService.API.Models;

public class Service
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public int EstimatedMinutes { get; set; } = 60;

    [JsonIgnore]
    public ICollection<OrderService> OrderServices { get; set; } = new List<OrderService>();
}