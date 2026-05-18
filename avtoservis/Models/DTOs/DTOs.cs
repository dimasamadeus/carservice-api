using CarService.API.Models;

namespace CarService.API.Models.DTOs;

public class ClientDto
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public class CarDto
{
    public string LicensePlate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
}

public class OrderDto
{
    public OrderStatus Status { get; set; }
    public int ClientId { get; set; }
    public int CarId { get; set; }
}

public class ServiceDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class PartDto
{
    public string Name { get; set; } = string.Empty;
    public string ArticleNumber { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? Category { get; set; }
}

public class OrderItemDto
{
    public int ItemId { get; set; }
    public int Quantity { get; set; } = 1;
}

public class FavoriteDto
{
    public int ServiceId { get; set; }
}
