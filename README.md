# 🔧 CarService API

REST API для управления автосервисом — ASP.NET Core 8, Entity Framework Core, SQLite.

**Команда:** Дмитрий · Павел

---

## 🚀 Запуск

### Docker (рекомендуется)
```bash
docker-compose up --build
```
Приложение: `http://localhost:8080` · Swagger: `http://localhost:8080/swagger`

### Локально
```bash
cd avtoservis
dotnet ef database update
dotnet run
```

---

## 🧪 Тесты
```bash
dotnet test avtoservis.Tests/avtoservis.Tests.csproj
```

---

## 🪝 Git Hook (автозапуск тестов)
```bash
sh INSTALL_HOOK.sh
```
После этого тесты запускаются автоматически перед каждым коммитом.

---

## 📡 Эндпоинты

| Метод | Маршрут | Описание |
|-------|---------|----------|
| GET | `/api/clients` | Список клиентов |
| POST | `/api/clients` | Создать клиента |
| GET | `/api/clients/{id}/cars` | Авто клиента |
| POST | `/api/clients/{id}/cars` | Добавить авто |
| GET | `/api/clients/{id}/favorites` | Избранные услуги |
| POST | `/api/clients/{id}/favorites` | Добавить в избранное |
| DELETE | `/api/clients/{id}/favorites/{serviceId}` | Убрать из избранного |
| GET | `/api/orders` | Список заказов |
| POST | `/api/orders` | Создать заказ |
| GET | `/api/orders/{id}/total` | Итоговая стоимость |
| POST | `/api/orders/{id}/items/services` | Добавить услугу в заказ |
| POST | `/api/orders/{id}/items/parts` | Добавить запчасть (контроль склада) |
| GET | `/api/services` | Список услуг |
| GET | `/api/parts` | Список запчастей (`?sortBy=price&order=desc`) |
| GET | `/api/parts/{id}/market-price` | Рыночная цена |

---

## 🛠️ Технологии

| Технология | Назначение |
|-----------|-----------|
| ASP.NET Core 8 | Веб-фреймворк |
| Entity Framework Core | ORM |
| SQLite | База данных |
| Swagger | Документация API |
| StyleCop.Analyzers | Линтер |
| xUnit | Unit-тесты |
| Docker | Контейнеризация |

---

## 👥 Авторы

| Участник | Зона ответственности |
|----------|---------------------|
| Дмитрий | Клиенты, автомобили, заказы, фронтенд |
| Павел | Услуги, запчасти, склад, избранное |
