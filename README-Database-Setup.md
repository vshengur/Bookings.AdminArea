# Настройка разделенных баз данных

Этот проект использует две базы данных:
- **PostgreSQL** - для bookings и booking states
- **MongoDB** - для hotels и rooms

## Архитектура

### PostgreSQL (Bookings и Booking States)
- **Назначение**: Хранение данных о бронированиях и их состояниях
- **Преимущества**: ACID транзакции, сложные запросы, связи между таблицами
- **Таблицы**:
  - `bookings` - основная информация о бронированиях
  - `booking_states` - состояния бронирований

### MongoDB (Hotels и Rooms)
- **Назначение**: Хранение данных об отелях и номерах
- **Преимущества**: Гибкая схема, масштабируемость, быстрые операции чтения
- **Коллекции**:
  - `Hotels` - информация об отелях
  - `Rooms` - информация о номерах

## Запуск с Docker

### 1. Запуск всех сервисов
```bash
docker-compose up -d
```

### 2. Проверка статуса
```bash
docker-compose ps
```

### 3. Просмотр логов
```bash
# Все сервисы
docker-compose logs

# Конкретный сервис
docker-compose logs postgres
docker-compose logs mongodb
docker-compose logs bookings.api
```

## Управление миграциями PostgreSQL

### Применение миграций
```powershell
# Применить все миграции
.\scripts\migrate-up.ps1

# С указанием строки подключения
.\scripts\migrate-up.ps1 -ConnectionString "Host=localhost;Database=bookings_db;Username=bookings_user;Password=bookings_password"
```

### Откат миграций
```powershell
# Откатить последнюю миграцию
.\scripts\migrate-down.ps1

# Откатить несколько миграций
.\scripts\migrate-down.ps1 -Steps 3
```

### Создание новой миграции
```powershell
# Создать новую миграцию
.\scripts\add-migration.ps1 "AddBookingStatusField"
```

## Ручное управление миграциями

### Применение миграций
```bash
cd Bookings.PostgresRepositories
dotnet ef database update
```

### Создание миграции
```bash
cd Bookings.PostgresRepositories
dotnet ef migrations add MigrationName
```

### Откат миграции
```bash
cd Bookings.PostgresRepositories
dotnet ef database update PreviousMigrationName
```

## Конфигурация

### appsettings.json
```json
{
  "ConnectionStrings": {
    "PostgresDatabase": "Host=localhost;Database=bookings_db;Username=bookings_user;Password=bookings_password",
    "MongoDatabase": "mongodb://admin:password@localhost:27017/bookings?authSource=admin"
  },
  "PostgresDatabase": {
    "ConnectionString": "Host=localhost;Database=bookings_db;Username=bookings_user;Password=bookings_password",
    "DatabaseName": "bookings_db"
  },
  "BookingsStoreDatabaseSettings": {
    "ConnectionString": "mongodb://admin:password@localhost:27017/bookings?authSource=admin",
    "DatabaseName": "bookings"
  }
}
```

### Переменные окружения для Docker
```bash
# PostgreSQL
POSTGRES_DB=bookings_db
POSTGRES_USER=bookings_user
POSTGRES_PASSWORD=bookings_password

# MongoDB
MONGO_INITDB_ROOT_USERNAME=admin
MONGO_INITDB_ROOT_PASSWORD=password
MONGO_INITDB_DATABASE=bookings
```

## Подключение к базам данных

### PostgreSQL
```bash
# Через Docker
docker exec -it bookings_postgres psql -U bookings_user -d bookings_db

# Локально (если порт 5432 открыт)
psql -h localhost -U bookings_user -d bookings_db
```

### MongoDB
```bash
# Через Docker
docker exec -it bookings_mongodb mongosh -u admin -p password

# Локально (если порт 27017 открыт)
mongosh mongodb://admin:password@localhost:27017/bookings?authSource=admin
```

## Структура проекта

```
Bookings.PostgresRepositories/
├── Contexts/
│   └── BookingsDbContext.cs
├── Migrations/
│   └── InitialCreate.cs
├── Models/
│   └── Settings/
│       └── PostgresDatabaseSettings.cs
├── Repositories/
│   ├── PostgresBookingsRepository.cs
│   └── PostgresBookingStateRepository.cs
├── BasePostgresRepository.cs
└── PostgresRepositoriesModule.cs

Bookings.Repositories/ (MongoDB)
├── Contexts/
├── Domain/
│   ├── HotelsRepository.cs
│   └── RoomsRepository.cs
└── RepositoriesModule.cs
```

## Мониторинг и отладка

### Проверка подключений
```bash
# PostgreSQL
docker exec bookings_postgres pg_isready -U bookings_user

# MongoDB
docker exec bookings_mongodb mongosh --eval "db.runCommand('ping')"
```

### Просмотр данных
```sql
-- PostgreSQL
SELECT * FROM bookings;
SELECT * FROM booking_states;
```

```javascript
// MongoDB
db.Hotels.find()
db.Rooms.find()
```

## Резервное копирование

### PostgreSQL
```bash
docker exec bookings_postgres pg_dump -U bookings_user bookings_db > backup.sql
```

### MongoDB
```bash
docker exec bookings_mongodb mongodump --username admin --password password --db bookings --out backup
```

## Восстановление

### PostgreSQL
```bash
docker exec -i bookings_postgres psql -U bookings_user -d bookings_db < backup.sql
```

### MongoDB
```bash
docker exec bookings_mongodb mongorestore --username admin --password password --db bookings backup/bookings
``` 