using CarService.API.Data;
using CarService.API.Models;
using CarService.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarService.API.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly AppDbContext _db;
    public ServicesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Services.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var s = await _db.Services.FindAsync(id);
        return s == null ? NotFound() : Ok(s);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ServiceDto dto)
    {
        var service = new Service { Name = dto.Name, Price = dto.Price };
        _db.Services.Add(service);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ServiceDto dto)
    {
        var s = await _db.Services.FindAsync(id);
        if (s == null) return NotFound();
        s.Name = dto.Name;
        s.Price = dto.Price;
        await _db.SaveChangesAsync();
        return Ok(s);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var s = await _db.Services.FindAsync(id);
        if (s == null) return NotFound();
        _db.Services.Remove(s);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

[ApiController]
[Route("api/parts")]
public class PartsController : ControllerBase
{
    private readonly AppDbContext _db;
    public PartsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? sortBy = null, [FromQuery] string? order = "asc")
    {
        IQueryable<Part> query = _db.Parts;
        var descending = order?.ToLower() == "desc";
        query = sortBy?.ToLower() switch
        {
            "price" => descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "name"  => descending ? query.OrderByDescending(p => p.Name)  : query.OrderBy(p => p.Name),
            _       => query.OrderBy(p => p.Id),
        };
        return Ok(await query.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var part = await _db.Parts.FindAsync(id);
        return part == null ? NotFound() : Ok(part);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PartDto dto)
    {
        var part = new Part { Name = dto.Name, ArticleNumber = dto.ArticleNumber, Price = dto.Price, Quantity = dto.Quantity, Category = dto.Category ?? string.Empty };
        _db.Parts.Add(part);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = part.Id }, part);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PartDto dto)
    {
        var part = await _db.Parts.FindAsync(id);
        if (part == null) return NotFound();
        part.Name = dto.Name;
        part.ArticleNumber = dto.ArticleNumber;
        part.Price = dto.Price;
        part.Quantity = dto.Quantity;
        part.Category = dto.Category ?? string.Empty;
        await _db.SaveChangesAsync();
        return Ok(part);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var part = await _db.Parts.FindAsync(id);
        if (part == null) return NotFound();
        _db.Parts.Remove(part);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id}/market-price")]
    public async Task<IActionResult> GetMarketPrice(int id)
    {
        var part = await _db.Parts.FindAsync(id);
        if (part == null) return NotFound();

        decimal marketPrice;
        string source;

        try
        {
            using var http = new HttpClient();
            http.Timeout = TimeSpan.FromSeconds(3);
            var response = await http.GetStringAsync("https://www.random.org/integers/?num=1&min=-20&max=20&col=1&base=10&format=plain&rnd=new");
            var pct = decimal.Parse(response.Trim());
            marketPrice = Math.Round(part.Price * (1 + (pct / 100m)), 2);
            source = "random.org";
        }
        catch
        {
            var rng = new Random((part.Id * 31) + DateTime.Now.DayOfYear);
            var pct = rng.Next(-20, 21);
            marketPrice = Math.Round(part.Price * (1 + (pct / 100m)), 2);
            source = "local-emulation";
        }

        return Ok(new { PartId = part.Id, Name = part.Name, Article = part.ArticleNumber, OurPrice = part.Price, MarketPrice = marketPrice, Difference = marketPrice - part.Price, DiffPercent = Math.Round(((marketPrice - part.Price) / part.Price) * 100, 1), Source = source });
    }
}

[ApiController]
[Route("api/orders/{orderId}/items")]
public class OrderItemsController : ControllerBase
{
    private readonly AppDbContext _db;
    public OrderItemsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetItems(int orderId)
    {
        if (!await _db.Orders.AnyAsync(o => o.Id == orderId)) return NotFound("Заказ не найден");

        var services = await _db.OrderServices.Where(os => os.OrderId == orderId).Include(os => os.Service)
            .Select(os => new { Type = "service", Id = os.Id, Name = os.Service!.Name, Price = os.Service.Price, Qty = 1 }).ToListAsync<object>();

        var parts = await _db.OrderParts.Where(op => op.OrderId == orderId).Include(op => op.Part)
            .Select(op => new { Type = "part", Id = op.Id, Name = op.Part!.Name, Price = op.PriceAtOrder, Qty = op.Quantity }).ToListAsync<object>();

        return Ok(new { Services = services, Parts = parts });
    }

    [HttpPost("services")]
    public async Task<IActionResult> AddService(int orderId, [FromBody] OrderItemDto dto)
    {
        if (!await _db.Orders.AnyAsync(o => o.Id == orderId)) return NotFound("Заказ не найден");
        var service = await _db.Services.FindAsync(dto.ItemId);
        if (service == null) return NotFound("Услуга не найдена");
        if (await _db.OrderServices.AnyAsync(os => os.OrderId == orderId && os.ServiceId == dto.ItemId))
            return Conflict("Услуга уже добавлена в заказ");
        _db.OrderServices.Add(new OrderService { OrderId = orderId, ServiceId = dto.ItemId });
        await _db.SaveChangesAsync();
        return Ok(new { message = $"Услуга «{service.Name}» добавлена в заказ #{orderId}" });
    }

    [HttpDelete("services/{serviceId}")]
    public async Task<IActionResult> RemoveService(int orderId, int serviceId)
    {
        var item = await _db.OrderServices.FirstOrDefaultAsync(os => os.OrderId == orderId && os.ServiceId == serviceId);
        if (item == null) return NotFound();
        _db.OrderServices.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("parts")]
    public async Task<IActionResult> AddPart(int orderId, [FromBody] OrderItemDto dto)
    {
        if (!await _db.Orders.AnyAsync(o => o.Id == orderId)) return NotFound("Заказ не найден");
        var part = await _db.Parts.FindAsync(dto.ItemId);
        if (part == null) return NotFound("Запчасть не найдена");

        var qty = dto.Quantity > 0 ? dto.Quantity : 1;
        if (part.Quantity < qty)
            return BadRequest(new { error = "Недостаточно запчастей на складе", partName = part.Name, required = qty, inStock = part.Quantity });

        part.Quantity -= qty;

        var existing = await _db.OrderParts.FirstOrDefaultAsync(op => op.OrderId == orderId && op.PartId == dto.ItemId);
        if (existing != null)
            existing.Quantity += qty;
        else
            _db.OrderParts.Add(new OrderPart { OrderId = orderId, PartId = dto.ItemId, Quantity = qty, PriceAtOrder = part.Price });

        await _db.SaveChangesAsync();
        return Ok(new { message = $"Запчасть «{part.Name}» x{qty} добавлена в заказ #{orderId}", remainsInStock = part.Quantity });
    }

    [HttpDelete("parts/{partId}")]
    public async Task<IActionResult> RemovePart(int orderId, int partId)
    {
        var item = await _db.OrderParts.FirstOrDefaultAsync(op => op.OrderId == orderId && op.PartId == partId);
        if (item == null) return NotFound();
        var part = await _db.Parts.FindAsync(partId);
        if (part != null) part.Quantity += item.Quantity;
        _db.OrderParts.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

[ApiController]
[Route("api/clients/{clientId}/favorites")]
public class FavoritesController : ControllerBase
{
    private readonly AppDbContext _db;
    public FavoritesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetFavorites(int clientId)
    {
        var client = await _db.Clients.FindAsync(clientId);
        if (client == null) return NotFound("Клиент не найден");
        var favorites = await _db.UserFavorites.Where(f => f.ClientId == clientId).Include(f => f.Service)
            .Select(f => new { FavoriteId = f.Id, ServiceId = f.ServiceId, Name = f.Service!.Name, Price = f.Service.Price, AddedAt = f.CreatedAt })
            .ToListAsync();
        return Ok(new { ClientId = clientId, ClientName = client.Name, Favorites = favorites, Count = favorites.Count });
    }

    [HttpPost]
    public async Task<IActionResult> AddFavorite(int clientId, [FromBody] FavoriteDto dto)
    {
        var client = await _db.Clients.FindAsync(clientId);
        if (client == null) return NotFound("Клиент не найден");
        var service = await _db.Services.FindAsync(dto.ServiceId);
        if (service == null) return NotFound("Услуга не найдена");
        if (await _db.UserFavorites.AnyAsync(f => f.ClientId == clientId && f.ServiceId == dto.ServiceId))
            return Conflict("Эта услуга уже в избранном");
        _db.UserFavorites.Add(new UserFavorite { ClientId = clientId, ServiceId = dto.ServiceId, CreatedAt = DateTime.UtcNow });
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetFavorites), new { clientId }, new { message = $"Услуга «{service.Name}» добавлена в избранное" });
    }

    [HttpDelete("{serviceId}")]
    public async Task<IActionResult> RemoveFavorite(int clientId, int serviceId)
    {
        var fav = await _db.UserFavorites.FirstOrDefaultAsync(f => f.ClientId == clientId && f.ServiceId == serviceId);
        if (fav == null) return NotFound("Запись не найдена в избранном");
        _db.UserFavorites.Remove(fav);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
