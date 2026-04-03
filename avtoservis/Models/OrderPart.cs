using System.Text.Json.Serialization;

namespace CarService.API.Models;

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