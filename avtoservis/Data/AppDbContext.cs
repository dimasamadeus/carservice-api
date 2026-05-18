using CarService.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CarService.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<OrderService> OrderServices => Set<OrderService>();
    public DbSet<OrderPart> OrderParts => Set<OrderPart>();
    public DbSet<UserFavorite> UserFavorites => Set<UserFavorite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>().HasData(
            new Client { Id = 1,  Name = "Алексей Петров",   Phone = "+7-900-111-0001" },
            new Client { Id = 2,  Name = "Мария Иванова",    Phone = "+7-900-111-0002" },
            new Client { Id = 3,  Name = "Дмитрий Смирнов",  Phone = "+7-900-111-0003" },
            new Client { Id = 4,  Name = "Елена Кузнецова",  Phone = "+7-900-111-0004" },
            new Client { Id = 5,  Name = "Сергей Попов",     Phone = "+7-900-111-0005" },
            new Client { Id = 6,  Name = "Ольга Новикова",   Phone = "+7-900-111-0006" },
            new Client { Id = 7,  Name = "Андрей Морозов",   Phone = "+7-900-111-0007" },
            new Client { Id = 8,  Name = "Татьяна Волкова",  Phone = "+7-900-111-0008" },
            new Client { Id = 9,  Name = "Николай Лебедев",  Phone = "+7-900-111-0009" },
            new Client { Id = 10, Name = "Анна Соколова",    Phone = "+7-900-111-0010" },
            new Client { Id = 11, Name = "Павел Козлов",     Phone = "+7-900-111-0011" },
            new Client { Id = 12, Name = "Виктория Орлова",  Phone = "+7-900-111-0012" });

        modelBuilder.Entity<Car>().HasData(
            new Car { Id = 1,  LicensePlate = "А001АА77", Brand = "Toyota",     Model = "Camry",     ClientId = 1  },
            new Car { Id = 2,  LicensePlate = "Б002ББ77", Brand = "Honda",      Model = "Civic",     ClientId = 1  },
            new Car { Id = 3,  LicensePlate = "В003ВВ77", Brand = "BMW",        Model = "X5",        ClientId = 2  },
            new Car { Id = 4,  LicensePlate = "Г004ГГ77", Brand = "Mercedes",   Model = "E-Class",   ClientId = 3  },
            new Car { Id = 5,  LicensePlate = "Д005ДД77", Brand = "Lada",       Model = "Vesta",     ClientId = 4  },
            new Car { Id = 6,  LicensePlate = "Е006ЕЕ77", Brand = "Kia",        Model = "Rio",       ClientId = 5  },
            new Car { Id = 7,  LicensePlate = "Ж007ЖЖ77", Brand = "Hyundai",    Model = "Solaris",   ClientId = 6  },
            new Car { Id = 8,  LicensePlate = "З008ЗЗ77", Brand = "Ford",       Model = "Focus",     ClientId = 7  },
            new Car { Id = 9,  LicensePlate = "И009ИИ77", Brand = "Volkswagen", Model = "Polo",      ClientId = 8  },
            new Car { Id = 10, LicensePlate = "К010КК77", Brand = "Nissan",     Model = "Qashqai",   ClientId = 9  },
            new Car { Id = 11, LicensePlate = "Л011ЛЛ77", Brand = "Renault",    Model = "Logan",     ClientId = 10 },
            new Car { Id = 12, LicensePlate = "М012ММ77", Brand = "Skoda",      Model = "Octavia",   ClientId = 11 },
            new Car { Id = 13, LicensePlate = "Н013НН77", Brand = "Mazda",      Model = "CX-5",      ClientId = 12 },
            new Car { Id = 14, LicensePlate = "О014ОО77", Brand = "Subaru",     Model = "Outback",   ClientId = 2  },
            new Car { Id = 15, LicensePlate = "П015ПП77", Brand = "Audi",       Model = "A4",        ClientId = 3  },
            new Car { Id = 16, LicensePlate = "Р016РР77", Brand = "Mitsubishi", Model = "Outlander", ClientId = 5  });

        modelBuilder.Entity<Service>().HasData(
            new Service { Id = 1,  Name = "Замена масла",            Price = 1500 },
            new Service { Id = 2,  Name = "Замена фильтра",          Price = 800  },
            new Service { Id = 3,  Name = "Диагностика двигателя",   Price = 2000 },
            new Service { Id = 4,  Name = "Замена тормозных колодок",Price = 2500 },
            new Service { Id = 5,  Name = "Шиномонтаж (1 колесо)",   Price = 400  },
            new Service { Id = 6,  Name = "Балансировка колес",      Price = 1200 },
            new Service { Id = 7,  Name = "Замена аккумулятора",     Price = 500  },
            new Service { Id = 8,  Name = "Компьютерная диагностика",Price = 1800 },
            new Service { Id = 9,  Name = "Замена ремня ГРМ",        Price = 5000 },
            new Service { Id = 10, Name = "Мойка двигателя",         Price = 3000 });

        modelBuilder.Entity<Part>().HasData(
            new Part { Id = 1,  Name = "Масло моторное 5W-30 4л",  ArticleNumber = "OIL-5W30-4",  Price = 2800, Quantity = 50, Category = "Масла"    },
            new Part { Id = 2,  Name = "Фильтр масляный Toyota",   ArticleNumber = "FLT-OIL-TOY", Price = 350,  Quantity = 30, Category = "Фильтры"  },
            new Part { Id = 3,  Name = "Фильтр воздушный",         ArticleNumber = "FLT-AIR-001", Price = 500,  Quantity = 25, Category = "Фильтры"  },
            new Part { Id = 4,  Name = "Тормозные колодки перед",  ArticleNumber = "BRK-PAD-F01", Price = 1800, Quantity = 20, Category = "Тормоза"  },
            new Part { Id = 5,  Name = "Тормозные колодки зад",    ArticleNumber = "BRK-PAD-R01", Price = 1500, Quantity = 20, Category = "Тормоза"  },
            new Part { Id = 6,  Name = "Аккумулятор 60Ah",         ArticleNumber = "BAT-60AH-001",Price = 5500, Quantity = 10, Category = "Электрика"},
            new Part { Id = 7,  Name = "Свечи зажигания (к-т 4)",  ArticleNumber = "SPK-PLG-004", Price = 800,  Quantity = 40, Category = "Электрика"},
            new Part { Id = 8,  Name = "Ремень ГРМ",               ArticleNumber = "TIM-BELT-001",Price = 2500, Quantity = 15, Category = "Двигатель"},
            new Part { Id = 9,  Name = "Охлаждающая жидкость 1л",  ArticleNumber = "COOL-FL-001", Price = 250,  Quantity = 60, Category = "Жидкости" },
            new Part { Id = 10, Name = "Фильтр топливный",         ArticleNumber = "FLT-FUEL-01", Price = 600,  Quantity = 25, Category = "Фильтры"  },
            new Part { Id = 11, Name = "Масло трансмиссионное",    ArticleNumber = "OIL-TRANS-01",Price = 1200, Quantity = 30, Category = "Масла"    },
            new Part { Id = 12, Name = "Амортизатор передний",     ArticleNumber = "SHCK-F-001",  Price = 3500, Quantity = 8,  Category = "Подвеска" });

        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1,  Status = OrderStatus.Completed,  ClientId = 1,  CarId = 1  },
            new Order { Id = 2,  Status = OrderStatus.Completed,  ClientId = 1,  CarId = 2  },
            new Order { Id = 3,  Status = OrderStatus.Completed,  ClientId = 2,  CarId = 3  },
            new Order { Id = 4,  Status = OrderStatus.InProgress, ClientId = 3,  CarId = 4  },
            new Order { Id = 5,  Status = OrderStatus.InProgress, ClientId = 4,  CarId = 5  },
            new Order { Id = 6,  Status = OrderStatus.Completed,  ClientId = 5,  CarId = 6  },
            new Order { Id = 7,  Status = OrderStatus.Completed,  ClientId = 6,  CarId = 7  },
            new Order { Id = 8,  Status = OrderStatus.Pending,    ClientId = 7,  CarId = 8  },
            new Order { Id = 9,  Status = OrderStatus.InProgress, ClientId = 8,  CarId = 9  },
            new Order { Id = 10, Status = OrderStatus.Completed,  ClientId = 9,  CarId = 10 },
            new Order { Id = 11, Status = OrderStatus.Completed,  ClientId = 10, CarId = 11 },
            new Order { Id = 12, Status = OrderStatus.Pending,    ClientId = 11, CarId = 12 },
            new Order { Id = 13, Status = OrderStatus.Completed,  ClientId = 12, CarId = 13 },
            new Order { Id = 14, Status = OrderStatus.Completed,  ClientId = 2,  CarId = 14 },
            new Order { Id = 15, Status = OrderStatus.InProgress, ClientId = 3,  CarId = 15 },
            new Order { Id = 16, Status = OrderStatus.Completed,  ClientId = 5,  CarId = 16 },
            new Order { Id = 17, Status = OrderStatus.Pending,    ClientId = 1,  CarId = 1  },
            new Order { Id = 18, Status = OrderStatus.Completed,  ClientId = 4,  CarId = 5  },
            new Order { Id = 19, Status = OrderStatus.InProgress, ClientId = 6,  CarId = 7  },
            new Order { Id = 20, Status = OrderStatus.Completed,  ClientId = 9,  CarId = 10 },
            new Order { Id = 21, Status = OrderStatus.Completed,  ClientId = 10, CarId = 11 },
            new Order { Id = 22, Status = OrderStatus.Pending,    ClientId = 12, CarId = 13 });

        modelBuilder.Entity<OrderService>().HasData(
            new OrderService { Id = 1,  OrderId = 1,  ServiceId = 1 },
            new OrderService { Id = 2,  OrderId = 1,  ServiceId = 2 },
            new OrderService { Id = 3,  OrderId = 2,  ServiceId = 3 },
            new OrderService { Id = 4,  OrderId = 3,  ServiceId = 4 },
            new OrderService { Id = 5,  OrderId = 4,  ServiceId = 8 },
            new OrderService { Id = 6,  OrderId = 5,  ServiceId = 1 },
            new OrderService { Id = 7,  OrderId = 6,  ServiceId = 5 },
            new OrderService { Id = 8,  OrderId = 6,  ServiceId = 6 },
            new OrderService { Id = 9,  OrderId = 7,  ServiceId = 9 },
            new OrderService { Id = 10, OrderId = 10, ServiceId = 1 },
            new OrderService { Id = 11, OrderId = 10, ServiceId = 2 },
            new OrderService { Id = 12, OrderId = 13, ServiceId = 7 });

        modelBuilder.Entity<OrderPart>().HasData(
            new OrderPart { Id = 1, OrderId = 1,  PartId = 1, Quantity = 1, PriceAtOrder = 2800 },
            new OrderPart { Id = 2, OrderId = 1,  PartId = 2, Quantity = 1, PriceAtOrder = 350  },
            new OrderPart { Id = 3, OrderId = 2,  PartId = 3, Quantity = 1, PriceAtOrder = 500  },
            new OrderPart { Id = 4, OrderId = 3,  PartId = 4, Quantity = 1, PriceAtOrder = 1800 },
            new OrderPart { Id = 5, OrderId = 5,  PartId = 1, Quantity = 1, PriceAtOrder = 2800 },
            new OrderPart { Id = 6, OrderId = 7,  PartId = 8, Quantity = 1, PriceAtOrder = 2500 },
            new OrderPart { Id = 7, OrderId = 10, PartId = 1, Quantity = 1, PriceAtOrder = 2800 },
            new OrderPart { Id = 8, OrderId = 10, PartId = 2, Quantity = 1, PriceAtOrder = 350  },
            new OrderPart { Id = 9, OrderId = 13, PartId = 6, Quantity = 1, PriceAtOrder = 5500 });
    }
}
