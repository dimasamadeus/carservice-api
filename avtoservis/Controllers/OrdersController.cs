using CarService.API.Data;
using CarService.API.Models;
using CarService.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarService.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    public OrdersController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _db.Orders
            .Include(o => o.Client)
            .Include(o => o.Car)
            .Select(o => new
            {
                o.Id,
                Status = o.Status.ToString(),
                Client = o.Client!.Name,
                Car = $"{o.Car!.Brand} {o.Car.Model} ({o.Car.LicensePlate})",
            })
            .ToListAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _db.Orders.Include(o => o.Client).Include(o => o.Car).FirstOrDefaultAsync(o => o.Id == id);
        return order == null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderDto dto)
    {
        var order = new Order { Status = dto.Status, ClientId = dto.ClientId, CarId = dto.CarId };
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] OrderDto dto)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order == null) return NotFound();
        order.Status = dto.Status;
        order.ClientId = dto.ClientId;
        order.CarId = dto.CarId;
        await _db.SaveChangesAsync();
        return Ok(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order == null) return NotFound();
        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id}/total")]
    public async Task<IActionResult> GetTotal(int id)
    {
        var order = await _db.Orders.Include(o => o.Client).Include(o => o.Car).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();

        var services = await _db.OrderServices
            .Where(os => os.OrderId == id).Include(os => os.Service)
            .Select(os => new { os.Service!.Name, os.Service.Price }).ToListAsync();

        var parts = await _db.OrderParts
            .Where(op => op.OrderId == id).Include(op => op.Part)
            .Select(op => new { op.Part!.Name, UnitPrice = op.PriceAtOrder, op.Quantity, TotalPrice = op.PriceAtOrder * op.Quantity })
            .ToListAsync();

        var servicesCost = services.Sum(s => s.Price);
        var partsCost    = parts.Sum(p => p.TotalPrice);

        return Ok(new
        {
            OrderId      = id,
            Status       = order.Status.ToString(),
            Client       = order.Client!.Name,
            Car          = $"{order.Car!.Brand} {order.Car.Model} ({order.Car.LicensePlate})",
            Services     = services,
            ServicesCost = servicesCost,
            Parts        = parts,
            PartsCost    = partsCost,
            TotalCost    = servicesCost + partsCost,
        });
    }
}
