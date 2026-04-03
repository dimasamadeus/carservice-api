using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarService.API.Data;
using CarService.API.Models;
using CarService.API.Models.DTOs;

namespace CarService.API.Controllers
{
    [ApiController]
    [Route("api/orders/{orderId}/items")]
    public class OrderItemsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public OrderItemsController(AppDbContext db) => _db = db;

        // GET /api/orders/{orderId}/items
        [HttpGet]
        public async Task<IActionResult> GetItems(int orderId)
        {
            if (!await _db.Orders.AnyAsync(o => o.Id == orderId))
                return NotFound("Заказ не найден");

            var services = await _db.OrderServices
                .Where(os => os.OrderId == orderId)
                .Include(os => os.Service)
                .Select(os => new
                {
                    Type  = "service",
                    Id    = os.Id,
                    Name  = os.Service!.Name,
                    Price = os.Service.Price,
                    Qty   = 1
                })
                .ToListAsync<object>();

            var parts = await _db.OrderParts
                .Where(op => op.OrderId == orderId)
                .Include(op => op.Part)
                .Select(op => new
                {
                    Type  = "part",
                    Id    = op.Id,
                    Name  = op.Part!.Name,
                    Price = op.PriceAtOrder,
                    Qty   = op.Quantity
                })
                .ToListAsync<object>();

            return Ok(new { Services = services, Parts = parts });
        }

        // POST /api/orders/{orderId}/items/services
        [HttpPost("services")]
        public async Task<IActionResult> AddService(int orderId, [FromBody] OrderItemDto dto)
        {
            if (!await _db.Orders.AnyAsync(o => o.Id == orderId))
                return NotFound("Заказ не найден");

            var service = await _db.Services.FindAsync(dto.ItemId);
            if (service == null) return NotFound("Услуга не найдена");

            var exists = await _db.OrderServices
                .AnyAsync(os => os.OrderId == orderId && os.ServiceId == dto.ItemId);
            if (exists) return Conflict("Услуга уже добавлена в заказ");

            _db.OrderServices.Add(new OrderService { OrderId = orderId, ServiceId = dto.ItemId });
            await _db.SaveChangesAsync();

            return Ok(new { message = $"Услуга «{service.Name}» добавлена в заказ #{orderId}" });
        }

        // DELETE /api/orders/{orderId}/items/services/{serviceId}
        [HttpDelete("services/{serviceId}")]
        public async Task<IActionResult> RemoveService(int orderId, int serviceId)
        {
            var item = await _db.OrderServices
                .FirstOrDefaultAsync(os => os.OrderId == orderId && os.ServiceId == serviceId);
            if (item == null) return NotFound();

            _db.OrderServices.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ===== Лабораторная 3: Контроль остатков =====

        // POST /api/orders/{orderId}/items/parts
        [HttpPost("parts")]
        public async Task<IActionResult> AddPart(int orderId, [FromBody] OrderItemDto dto)
        {
            if (!await _db.Orders.AnyAsync(o => o.Id == orderId))
                return NotFound("Заказ не найден");

            var part = await _db.Parts.FindAsync(dto.ItemId);
            if (part == null) return NotFound("Запчасть не найдена");

            int qty = dto.Quantity > 0 ? dto.Quantity : 1;

            // Проверка остатков на складе
            if (part.Quantity < qty)
            {
                return BadRequest(new
                {
                    error    = "Недостаточно запчастей на складе",
                    partName = part.Name,
                    required = qty,
                    inStock  = part.Quantity
                });
            }

            // Уменьшаем остаток на складе
            part.Quantity -= qty;

            // Если такая запчасть уже есть в заказе — увеличиваем количество
            var existing = await _db.OrderParts
                .FirstOrDefaultAsync(op => op.OrderId == orderId && op.PartId == dto.ItemId);

            if (existing != null)
            {
                existing.Quantity += qty;
                // PriceAtOrder остаётся от первого добавления
            }
            else
            {
                _db.OrderParts.Add(new OrderPart
                {
                    OrderId      = orderId,
                    PartId       = dto.ItemId,
                    Quantity     = qty,
                    PriceAtOrder = part.Price   // фиксируем цену на момент добавления
                });
            }

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message        = $"Запчасть «{part.Name}» x{qty} добавлена в заказ #{orderId}",
                remainsInStock = part.Quantity
            });
        }

        // DELETE /api/orders/{orderId}/items/parts/{partId}  — возврат на склад
        [HttpDelete("parts/{partId}")]
        public async Task<IActionResult> RemovePart(int orderId, int partId)
        {
            var item = await _db.OrderParts
                .FirstOrDefaultAsync(op => op.OrderId == orderId && op.PartId == partId);
            if (item == null) return NotFound();

            // Возвращаем на склад
            var part = await _db.Parts.FindAsync(partId);
            if (part != null) part.Quantity += item.Quantity;

            _db.OrderParts.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
