using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarService.API.Models;

public class UserFavorite
{
    public int Id { get; set; }

    [Required]
    public int ClientId { get; set; }

    [Required]
    public int ServiceId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public Client? Client { get; set; }

    [JsonIgnore]
    public Service? Service { get; set; }
}