// ===== OrderDto.cs =====
// Используйте OrderStatus enum, как у вас в проекте
using CarService.API.Models;

namespace CarService.API.Models.DTOs
{
    public class OrderDto
    {
        public OrderStatus Status   { get; set; }
        public int         ClientId { get; set; }
        public int         CarId    { get; set; }
    }
}
