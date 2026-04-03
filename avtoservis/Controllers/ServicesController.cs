using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarService.API.Data;
using CarService.API.Models;
using CarService.API.Models.DTOs;

namespace CarService.API.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServicesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ServicesController(AppDbContext db) => _db = db;

        // GET /api/services
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _db.Services.ToListAsync());

        // GET /api/services/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var s = await _db.Services.FindAsync(id);
            return s == null ? NotFound() : Ok(s);
        }

        // POST /api/services
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceDto dto)
        {
            var service = new Service { Name = dto.Name, Price = dto.Price };
            _db.Services.Add(service);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
        }

        // PUT /api/services/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceDto dto)
        {
            var s = await _db.Services.FindAsync(id);
            if (s == null) return NotFound();
            s.Name  = dto.Name;
            s.Price = dto.Price;
            await _db.SaveChangesAsync();
            return Ok(s);
        }

        // DELETE /api/services/{id}
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
}
