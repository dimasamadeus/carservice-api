using avtoservis;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarService.API.Models;

public class Part
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ArticleNumber { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public string? CompatibleBrands { get; set; }

    [JsonIgnore]
    public ICollection<OrderPart> OrderParts { get; set; } = new List<OrderPart>();
}