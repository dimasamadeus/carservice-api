using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarService.API.Data;
using CarService.API.Models;
using CarService.API.Models.DTOs;

namespace CarService.API.Controllers
{
    /// <summary>
    /// Лабораторная 4: Избранные услуги клиента
    /// (UserFavorite хранит ClientId + ServiceId)
    /// </summary>
    [ApiController]
    [Route("api/clients/{clientId}/favorites")]
    public class FavoritesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public FavoritesController(AppDbContext db) => _db = db;

        // GET /api/clients/{clientId}/favorites
        [HttpGet]
        public async Task<IActionResult> GetFavorites(int clientId)
        {
            var client = await _db.Clients.FindAsync(clientId);
            if (client == null) return NotFound("Клиент не найден");

            var favorites = await _db.UserFavorites
                .Where(f => f.ClientId == clientId)
                .Include(f => f.Service)
                .Select(f => new
                {
                    FavoriteId = f.Id,
                    ServiceId  = f.ServiceId,
                    Name       = f.Service!.Name,
                    Price      = f.Service.Price,
                    AddedAt    = f.CreatedAt
                })
                .ToListAsync();

            return Ok(new
            {
                ClientId   = clientId,
                ClientName = client.Name,
                Favorites  = favorites,
                Count      = favorites.Count
            });
        }

        // POST /api/clients/{clientId}/favorites
        [HttpPost]
        public async Task<IActionResult> AddFavorite(int clientId, [FromBody] FavoriteDto dto)
        {
            var client = await _db.Clients.FindAsync(clientId);
            if (client == null) return NotFound("Клиент не найден");

            var service = await _db.Services.FindAsync(dto.ServiceId);
            if (service == null) return NotFound("Услуга не найдена");

            // Проверяем дубли
            var exists = await _db.UserFavorites
                .AnyAsync(f => f.ClientId == clientId && f.ServiceId == dto.ServiceId);
            if (exists)
                return Conflict("Эта услуга уже в избранном у клиента");

            _db.UserFavorites.Add(new UserFavorite
            {
                ClientId  = clientId,
                ServiceId = dto.ServiceId,
                CreatedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFavorites), new { clientId },
                new { message = $"Услуга «{service.Name}» добавлена в избранное" });
        }

        // DELETE /api/clients/{clientId}/favorites/{serviceId}
        [HttpDelete("{serviceId}")]
        public async Task<IActionResult> RemoveFavorite(int clientId, int serviceId)
        {
            var fav = await _db.UserFavorites
                .FirstOrDefaultAsync(f => f.ClientId == clientId && f.ServiceId == serviceId);

            if (fav == null) return NotFound("Запись не найдена в избранном");

            _db.UserFavorites.Remove(fav);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
