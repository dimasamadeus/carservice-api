using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarService.API.Models;

public class Client
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Phone { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Car> Cars { get; set; } = new List<Car>();
}

public class Car
{
    public int Id { get; set; }

    [Required]
    public string LicensePlate { get; set; } = string.Empty;

    [Required]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;

    public int ClientId { get; set; }

    [JsonIgnore]
    public Client? Client { get; set; }
}

public enum OrderStatus
{
    Pending,
    InProgress,
    Completed,
}

public class Order
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    public int ClientId { get; set; }
    public int CarId { get; set; }

    [JsonIgnore]
    public Client? Client { get; set; }

    [JsonIgnore]
    public Car? Car { get; set; }
}

public class Service
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }
}

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

public class OrderService
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ServiceId { get; set; }

    [JsonIgnore]
    public Order? Order { get; set; }

    [JsonIgnore]
    public Service? Service { get; set; }
}

public class OrderPart
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int PartId { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtOrder { get; set; }

    [JsonIgnore]
    public Order? Order { get; set; }

    [JsonIgnore]
    public Part? Part { get; set; }
}

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
