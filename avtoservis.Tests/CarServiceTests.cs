using CarService.API.Controllers;
using CarService.API.Data;
using CarService.API.Models;
using CarService.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace avtoservis.Tests;

public class PartsControllerTests
{
    private AppDbContext CreateDb() => new(
        new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    [Fact]
    public async Task GetAll_ReturnsAllParts()
    {
        using var db = CreateDb();
        db.Parts.AddRange(
            new Part { Name = "Масло",  ArticleNumber = "A", Price = 2800, Quantity = 10, Category = "Масла"   },
            new Part { Name = "Фильтр", ArticleNumber = "B", Price = 500,  Quantity = 20, Category = "Фильтры" });
        await db.SaveChangesAsync();

        var result = await new PartsController(db).GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(2, ((IEnumerable<Part>)ok.Value!).Count());
    }

    [Fact]
    public async Task GetById_NotFound_Returns404()
    {
        using var db = CreateDb();
        var result = await new PartsController(db).GetById(999);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ValidPart_Returns201()
    {
        using var db = CreateDb();
        var result = await new PartsController(db).Create(new PartDto { Name = "Новая", ArticleNumber = "X", Price = 1000, Quantity = 5 });
        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(1, await db.Parts.CountAsync());
    }

    [Fact]
    public async Task Delete_ExistingPart_Returns204()
    {
        using var db = CreateDb();
        db.Parts.Add(new Part { Id = 1, Name = "Масло", ArticleNumber = "A", Price = 2800, Quantity = 10, Category = "Масла" });
        await db.SaveChangesAsync();
        var result = await new PartsController(db).Delete(1);
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(0, await db.Parts.CountAsync());
    }

    [Fact]
    public async Task GetAll_SortByPriceAsc_ReturnsSorted()
    {
        using var db = CreateDb();
        db.Parts.AddRange(
            new Part { Name = "Дорогая", ArticleNumber = "A", Price = 5000, Quantity = 1, Category = "X" },
            new Part { Name = "Дешёвая", ArticleNumber = "B", Price = 500,  Quantity = 1, Category = "X" });
        await db.SaveChangesAsync();

        var result = await new PartsController(db).GetAll(sortBy: "price", order: "asc");
        var parts = ((IEnumerable<Part>)((OkObjectResult)result).Value!).ToList();

        Assert.Equal(500,  parts[0].Price);
        Assert.Equal(5000, parts[1].Price);
    }
}

public class OrderItemsControllerTests
{
    private async Task<AppDbContext> CreateDbWithData()
    {
        var db = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

        db.Clients.Add(new Client { Id = 1, Name = "Тест", Phone = "123" });
        db.Cars.Add(new Car { Id = 1, LicensePlate = "А001", Brand = "Toyota", Model = "Camry", ClientId = 1 });
        db.Orders.Add(new Order { Id = 1, Status = OrderStatus.Pending, ClientId = 1, CarId = 1 });
        db.Parts.Add(new Part { Id = 1, Name = "Масло", ArticleNumber = "OIL", Price = 2800, Quantity = 10, Category = "Масла" });
        db.Services.Add(new Service { Id = 1, Name = "Замена масла", Price = 1500 });
        await db.SaveChangesAsync();
        return db;
    }

    [Fact]
    public async Task AddPart_SufficientStock_DecreasesQuantity()
    {
        using var db = await CreateDbWithData();
        var result = await new OrderItemsController(db).AddPart(1, new OrderItemDto { ItemId = 1, Quantity = 3 });
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(7, (await db.Parts.FindAsync(1))!.Quantity);
    }

    [Fact]
    public async Task AddPart_InsufficientStock_Returns400()
    {
        using var db = await CreateDbWithData();
        var result = await new OrderItemsController(db).AddPart(1, new OrderItemDto { ItemId = 1, Quantity = 50 });
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(10, (await db.Parts.FindAsync(1))!.Quantity); // склад не изменился
    }

    [Fact]
    public async Task RemovePart_RestoresStock()
    {
        using var db = await CreateDbWithData();
        db.OrderParts.Add(new OrderPart { OrderId = 1, PartId = 1, Quantity = 3, PriceAtOrder = 2800 });
        var part = await db.Parts.FindAsync(1);
        part!.Quantity = 7;
        await db.SaveChangesAsync();

        var result = await new OrderItemsController(db).RemovePart(1, 1);
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(10, (await db.Parts.FindAsync(1))!.Quantity);
    }

    [Fact]
    public async Task AddPart_OrderNotFound_Returns404()
    {
        using var db = await CreateDbWithData();
        var result = await new OrderItemsController(db).AddPart(999, new OrderItemDto { ItemId = 1, Quantity = 1 });
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task AddService_Duplicate_Returns409()
    {
        using var db = await CreateDbWithData();
        db.OrderServices.Add(new OrderService { OrderId = 1, ServiceId = 1 });
        await db.SaveChangesAsync();
        var result = await new OrderItemsController(db).AddService(1, new OrderItemDto { ItemId = 1 });
        Assert.IsType<ConflictObjectResult>(result);
    }
}

public class ServicesControllerTests
{
    private AppDbContext CreateDb() => new(
        new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    [Fact]
    public async Task Create_ValidService_SavedToDb()
    {
        using var db = CreateDb();
        await new ServicesController(db).Create(new ServiceDto { Name = "Замена масла", Price = 1500 });
        Assert.Equal(1, await db.Services.CountAsync());
    }

    [Fact]
    public async Task Update_ExistingService_UpdatesFields()
    {
        using var db = CreateDb();
        db.Services.Add(new Service { Id = 1, Name = "Старое", Price = 1000 });
        await db.SaveChangesAsync();
        await new ServicesController(db).Update(1, new ServiceDto { Name = "Новое", Price = 2000 });
        var s = await db.Services.FindAsync(1);
        Assert.Equal("Новое", s!.Name);
        Assert.Equal(2000, s.Price);
    }
}
