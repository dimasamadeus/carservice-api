using System.Text.Json.Serialization;

namespace CarService.API.Models;

public class OrderService
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ServiceId { get; set; }
    public decimal PriceAtOrder { get; set; }

    [JsonIgnore]
    public Order? Order { get; set; }

    [JsonIgnore]
    public Service? Service { get; set; }
}