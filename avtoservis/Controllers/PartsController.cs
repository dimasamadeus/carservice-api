using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarService.API.Data;
using CarService.API.Models;
using CarService.API.Models.DTOs;

namespace CarService.API.Controllers
{
    [ApiController]
    [Route("api/parts")]
    public class PartsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PartsController(AppDbContext db) => _db = db;

        // GET /api/parts?sortBy=price|name&order=asc|desc  (Лабораторная 3)
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? sortBy = null,
            [FromQuery] string? order  = "asc")
        {
            IQueryable<Part> query = _db.Parts;

            bool descending = order?.ToLower() == "desc";

            query = sortBy?.ToLower() switch
            {
                "price" => descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "name"  => descending ? query.OrderByDescending(p => p.Name)  : query.OrderBy(p => p.Name),
                _       => query.OrderBy(p => p.Id)
            };

            return Ok(await query.ToListAsync());
        }

        // GET /api/parts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var part = await _db.Parts.FindAsync(id);
            return part == null ? NotFound() : Ok(part);
        }

        // POST /api/parts
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PartDto dto)
        {
            var part = new Part
            {
                Name          = dto.Name,
                ArticleNumber = dto.ArticleNumber,
                Price         = dto.Price,
                Quantity      = dto.Quantity,
                Category      = dto.Category ?? string.Empty
            };
            _db.Parts.Add(part);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = part.Id }, part);
        }

        // PUT /api/parts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PartDto dto)
        {
            var part = await _db.Parts.FindAsync(id);
            if (part == null) return NotFound();

            part.Name          = dto.Name;
            part.ArticleNumber = dto.ArticleNumber;
            part.Price         = dto.Price;
            part.Quantity      = dto.Quantity;
            part.Category      = dto.Category ?? string.Empty;
            await _db.SaveChangesAsync();
            return Ok(part);
        }

        // DELETE /api/parts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var part = await _db.Parts.FindAsync(id);
            if (part == null) return NotFound();
            _db.Parts.Remove(part);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ===== Лабораторная 3: Внешнее API (эмуляция рыночной цены) =====

        /// <summary>
        /// Получает рыночную цену запчасти через внешний API (эмулируется через random.org)
        /// </summary>
        [HttpGet("{id}/market-price")]
        public async Task<IActionResult> GetMarketPrice(int id)
        {
            var part = await _db.Parts.FindAsync(id);
            if (part == null) return NotFound();

            decimal marketPrice;
            string  source;

            try
            {
                using var http = new HttpClient();
                http.Timeout = TimeSpan.FromSeconds(3);
                var response = await http.GetStringAsync(
                    "https://www.random.org/integers/?num=1&min=-20&max=20&col=1&base=10&format=plain&rnd=new");
                var pct = decimal.Parse(response.Trim());
                marketPrice = Math.Round(part.Price * (1 + pct / 100m), 2);
                source      = "random.org";
            }
            catch
            {
                // Локальный fallback если внешний API недоступен
                var rng = new Random(part.Id * 31 + DateTime.Now.DayOfYear);
                var pct = rng.Next(-20, 21);
                marketPrice = Math.Round(part.Price * (1 + pct / 100m), 2);
                source      = "local-emulation";
            }

            return Ok(new
            {
                PartId      = part.Id,
                Name        = part.Name,
                Article     = part.ArticleNumber,
                OurPrice    = part.Price,
                MarketPrice = marketPrice,
                Difference  = marketPrice - part.Price,
                DiffPercent = Math.Round((marketPrice - part.Price) / part.Price * 100, 1),
                Source      = source
            });
        }
    }
}
