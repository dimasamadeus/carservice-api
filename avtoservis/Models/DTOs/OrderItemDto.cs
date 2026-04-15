// ===== OrderItemDto.cs =====
namespace CarService.API.Models.DTOs
{
    public class OrderItemDto
    {
        /// <summary>ID услуги или запчасти</summary>
        public int ItemId   { get; set; }
        /// <summary>Количество (для запчастей; для услуг игнорируется)</summary>
        public int Quantity { get; set; } = 1;
    }
}
