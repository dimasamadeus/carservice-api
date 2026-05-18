using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace avtoservis.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ArticleNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CompatibleBrands = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LicensePlate = table.Column<string>(type: "TEXT", nullable: false),
                    Brand = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFavorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFavorites_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavorites_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    CarId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    PartId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    PriceAtOrder = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderParts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderParts_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderServices_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "Алексей Петров", "+7-900-111-0001" },
                    { 2, "Мария Иванова", "+7-900-111-0002" },
                    { 3, "Дмитрий Смирнов", "+7-900-111-0003" },
                    { 4, "Елена Кузнецова", "+7-900-111-0004" },
                    { 5, "Сергей Попов", "+7-900-111-0005" },
                    { 6, "Ольга Новикова", "+7-900-111-0006" },
                    { 7, "Андрей Морозов", "+7-900-111-0007" },
                    { 8, "Татьяна Волкова", "+7-900-111-0008" },
                    { 9, "Николай Лебедев", "+7-900-111-0009" },
                    { 10, "Анна Соколова", "+7-900-111-0010" },
                    { 11, "Павел Козлов", "+7-900-111-0011" },
                    { 12, "Виктория Орлова", "+7-900-111-0012" }
                });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "ArticleNumber", "Category", "CompatibleBrands", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, "OIL-5W30-4", "Масла", null, "Масло моторное 5W-30 4л", 2800m, 50 },
                    { 2, "FLT-OIL-TOY", "Фильтры", null, "Фильтр масляный Toyota", 350m, 30 },
                    { 3, "FLT-AIR-001", "Фильтры", null, "Фильтр воздушный", 500m, 25 },
                    { 4, "BRK-PAD-F01", "Тормоза", null, "Тормозные колодки перед", 1800m, 20 },
                    { 5, "BRK-PAD-R01", "Тормоза", null, "Тормозные колодки зад", 1500m, 20 },
                    { 6, "BAT-60AH-001", "Электрика", null, "Аккумулятор 60Ah", 5500m, 10 },
                    { 7, "SPK-PLG-004", "Электрика", null, "Свечи зажигания (к-т 4)", 800m, 40 },
                    { 8, "TIM-BELT-001", "Двигатель", null, "Ремень ГРМ", 2500m, 15 },
                    { 9, "COOL-FL-001", "Жидкости", null, "Охлаждающая жидкость 1л", 250m, 60 },
                    { 10, "FLT-FUEL-01", "Фильтры", null, "Фильтр топливный", 600m, 25 },
                    { 11, "OIL-TRANS-01", "Масла", null, "Масло трансмиссионное", 1200m, 30 },
                    { 12, "SHCK-F-001", "Подвеска", null, "Амортизатор передний", 3500m, 8 }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Замена масла", 1500m },
                    { 2, "Замена фильтра", 800m },
                    { 3, "Диагностика двигателя", 2000m },
                    { 4, "Замена тормозных колодок", 2500m },
                    { 5, "Шиномонтаж (1 колесо)", 400m },
                    { 6, "Балансировка колес", 1200m },
                    { 7, "Замена аккумулятора", 500m },
                    { 8, "Компьютерная диагностика", 1800m },
                    { 9, "Замена ремня ГРМ", 5000m },
                    { 10, "Мойка двигателя", 3000m }
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Brand", "ClientId", "LicensePlate", "Model" },
                values: new object[,]
                {
                    { 1, "Toyota", 1, "А001АА77", "Camry" },
                    { 2, "Honda", 1, "Б002ББ77", "Civic" },
                    { 3, "BMW", 2, "В003ВВ77", "X5" },
                    { 4, "Mercedes", 3, "Г004ГГ77", "E-Class" },
                    { 5, "Lada", 4, "Д005ДД77", "Vesta" },
                    { 6, "Kia", 5, "Е006ЕЕ77", "Rio" },
                    { 7, "Hyundai", 6, "Ж007ЖЖ77", "Solaris" },
                    { 8, "Ford", 7, "З008ЗЗ77", "Focus" },
                    { 9, "Volkswagen", 8, "И009ИИ77", "Polo" },
                    { 10, "Nissan", 9, "К010КК77", "Qashqai" },
                    { 11, "Renault", 10, "Л011ЛЛ77", "Logan" },
                    { 12, "Skoda", 11, "М012ММ77", "Octavia" },
                    { 13, "Mazda", 12, "Н013НН77", "CX-5" },
                    { 14, "Subaru", 2, "О014ОО77", "Outback" },
                    { 15, "Audi", 3, "П015ПП77", "A4" },
                    { 16, "Mitsubishi", 5, "Р016РР77", "Outlander" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CarId", "ClientId", "Status" },
                values: new object[,]
                {
                    { 1, 1, 1, 2 },
                    { 2, 2, 1, 2 },
                    { 3, 3, 2, 2 },
                    { 4, 4, 3, 1 },
                    { 5, 5, 4, 1 },
                    { 6, 6, 5, 2 },
                    { 7, 7, 6, 2 },
                    { 8, 8, 7, 0 },
                    { 9, 9, 8, 1 },
                    { 10, 10, 9, 2 },
                    { 11, 11, 10, 2 },
                    { 12, 12, 11, 0 },
                    { 13, 13, 12, 2 },
                    { 14, 14, 2, 2 },
                    { 15, 15, 3, 1 },
                    { 16, 16, 5, 2 },
                    { 17, 1, 1, 0 },
                    { 18, 5, 4, 2 },
                    { 19, 7, 6, 1 },
                    { 20, 10, 9, 2 },
                    { 21, 11, 10, 2 },
                    { 22, 13, 12, 0 }
                });

            migrationBuilder.InsertData(
                table: "OrderParts",
                columns: new[] { "Id", "OrderId", "PartId", "PriceAtOrder", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 2800m, 1 },
                    { 2, 1, 2, 350m, 1 },
                    { 3, 2, 3, 500m, 1 },
                    { 4, 3, 4, 1800m, 1 },
                    { 5, 5, 1, 2800m, 1 },
                    { 6, 7, 8, 2500m, 1 },
                    { 7, 10, 1, 2800m, 1 },
                    { 8, 10, 2, 350m, 1 },
                    { 9, 13, 6, 5500m, 1 }
                });

            migrationBuilder.InsertData(
                table: "OrderServices",
                columns: new[] { "Id", "OrderId", "ServiceId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 2, 3 },
                    { 4, 3, 4 },
                    { 5, 4, 8 },
                    { 6, 5, 1 },
                    { 7, 6, 5 },
                    { 8, 6, 6 },
                    { 9, 7, 9 },
                    { 10, 10, 1 },
                    { 11, 10, 2 },
                    { 12, 13, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ClientId",
                table: "Cars",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderParts_OrderId",
                table: "OrderParts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderParts_PartId",
                table: "OrderParts",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CarId",
                table: "Orders",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderServices_OrderId",
                table: "OrderServices",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderServices_ServiceId",
                table: "OrderServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorites_ClientId",
                table: "UserFavorites",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorites_ServiceId",
                table: "UserFavorites",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderParts");

            migrationBuilder.DropTable(
                name: "OrderServices");

            migrationBuilder.DropTable(
                name: "UserFavorites");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
