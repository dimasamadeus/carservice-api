using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarService.API.Data;
using CarService.API.Models;
using CarService.API.Models.DTOs;

namespace CarService.API.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ClientsController(AppDbContext db) => _db = db;

        // GET /api/clients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _db.Clients
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Phone,
                    CarsCount = _db.Cars.Count(car => car.ClientId == c.Id)
                })
                .ToListAsync();
            return Ok(clients);
        }

        // GET /api/clients/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _db.Clients.FindAsync(id);
            return client == null ? NotFound() : Ok(client);
        }

        // GET /api/clients/{id}/cars
        [HttpGet("{id}/cars")]
        public async Task<IActionResult> GetCars(int id)
        {
            if (!await _db.Clients.AnyAsync(c => c.Id == id))
                return NotFound("Клиент не найден");

            var cars = await _db.Cars
                .Where(c => c.ClientId == id)
                .ToListAsync();
            return Ok(cars);
        }

        // POST /api/clients
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientDto dto)
        {
            var client = new Client { Name = dto.Name, Phone = dto.Phone };
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        // POST /api/clients/{id}/cars
        [HttpPost("{id}/cars")]
        public async Task<IActionResult> AddCar(int id, [FromBody] CarDto dto)
        {
            if (!await _db.Clients.AnyAsync(c => c.Id == id))
                return NotFound("Клиент не найден");

            var plateExists = await _db.Cars.AnyAsync(c => c.LicensePlate == dto.LicensePlate);
            if (plateExists) return Conflict($"Автомобиль с номером {dto.LicensePlate} уже существует");

            var car = new Car
            {
                LicensePlate = dto.LicensePlate,
                Brand        = dto.Brand,
                Model        = dto.Model,
                ClientId     = id
            };
            _db.Cars.Add(car);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCars), new { id }, car);
        }

        // PUT /api/clients/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClientDto dto)
        {
            var client = await _db.Clients.FindAsync(id);
            if (client == null) return NotFound();
            client.Name  = dto.Name;
            client.Phone = dto.Phone;
            await _db.SaveChangesAsync();
            return Ok(client);
        }

        // DELETE /api/clients/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _db.Clients.FindAsync(id);
            if (client == null) return NotFound();
            _db.Clients.Remove(client);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
