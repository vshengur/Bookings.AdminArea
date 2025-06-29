# Доменный слой - Чистая архитектура с FluentValidation

## Обзор

Доменный слой реализован в соответствии с принципами Domain-Driven Design (DDD) и чистой архитектуры. Все доменные объекты являются иммутабельными `record` типами. Валидация вынесена из конструкторов и реализована с использованием FluentValidation в цепочке обработчиков команд.

## Архитектурные принципы

### 1. Иммутабельность
- Все доменные объекты реализованы как `record` типы
- Свойства имеют модификатор `init`, что обеспечивает иммутабельность после создания
- Изменения создают новые экземпляры объектов

### 2. Разделение ответственности
- Доменные объекты содержат только бизнес-логику
- Валидация вынесена в отдельные валидаторы FluentValidation
- Команды и обработчики для операций создания/изменения

### 3. Валидация через FluentValidation
- Валидация происходит на уровне команд
- Использование декораторов для автоматической валидации
- Богатые возможности валидации с кастомными правилами

### 4. Паттерн Command/Handler
- Команды для инкапсуляции операций
- Обработчики для выполнения бизнес-логики
- Декораторы для cross-cutting concerns

## Структура проекта

```
Bookings.Domain/
├── BaseObject.cs                    # Базовый доменный объект
├── Booking.cs                       # Доменная сущность бронирования
├── Hotel.cs                         # Доменная сущность отеля
├── Room.cs                          # Доменная сущность номера
├── Commands/                        # Команды для операций
│   ├── CreateHotelCommand.cs
│   ├── CreateRoomCommand.cs
│   └── CreateBookingCommand.cs
├── Validators/                      # Валидаторы FluentValidation
│   ├── CreateHotelCommandValidator.cs
│   ├── CreateRoomCommandValidator.cs
│   └── CreateBookingCommandValidator.cs
├── Handlers/                        # Обработчики команд
│   ├── ICommandHandler.cs
│   ├── ICreateHotelCommandHandler.cs
│   ├── ICreateRoomCommandHandler.cs
│   ├── ICreateBookingCommandHandler.cs
│   ├── ValidationCommandHandlerDecorator.cs
│   └── Implementations/
│       └── CreateHotelCommandHandler.cs
└── README.md
```

## Базовый класс BaseObject

```csharp
public abstract record BaseObject : IBaseObject
{
    public string Id { get; init; }
    public DateTime Created { get; init; }
    public DateTime? EditedAt { get; private set; }
    
    // Автоматическая генерация ID и времени создания
    protected BaseObject()
    
    // Конструктор с базовыми параметрами
    protected BaseObject(string id, DateTime created)
    
    // Методы для работы с временем обновления
    public void MarkAsUpdated()
    protected BaseObject WithUpdated()
}
```

## Команды

### CreateHotelCommand
```csharp
public record CreateHotelCommand
{
    public string Name { get; init; }
    public string City { get; init; }
    public string Country { get; init; }
    public int Stars { get; init; }
    public int Rate { get; init; }
    public double LocationX { get; init; }
    public double LocationY { get; init; }
}
```

### CreateRoomCommand
```csharp
public record CreateRoomCommand
{
    public string Name { get; init; }
    public int MaxPersons { get; init; }
    public int AdditionalFreeKids { get; init; }
    public string HotelId { get; init; }
}
```

### CreateBookingCommand
```csharp
public record CreateBookingCommand
{
    public string BookName { get; init; }
    public string RoomId { get; init; }
    public double Price { get; init; }
    public string Category { get; init; }
    public Guid StateId { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public int Adults { get; init; }
    public int Kids { get; init; }
}
```

## Валидация с FluentValidation

### CreateHotelCommandValidator
```csharp
public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название отеля не может быть пустым")
            .MaximumLength(100)
            .WithMessage("Название отеля не может превышать 100 символов");

        RuleFor(x => x.Stars)
            .InclusiveBetween(1, 5)
            .WithMessage("Количество звезд должно быть от 1 до 5");

        RuleFor(x => x.Rate)
            .InclusiveBetween(0, 10)
            .WithMessage("Рейтинг должен быть от 0 до 10");

        // Валидация координат
        RuleFor(x => x.LocationX)
            .InclusiveBetween(-180, 180)
            .WithMessage("Долгота должна быть в диапазоне от -180 до 180");
    }
}
```

### CreateBookingCommandValidator
```csharp
public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.StartDate)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Дата начала должна быть в будущем");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("Дата окончания должна быть позже даты начала");

        // Бизнес-правило: общее количество людей не может превышать 20
        RuleFor(x => x)
            .Must(x => x.Adults + x.Kids <= 20)
            .WithMessage("Общее количество людей не может превышать 20");
    }
}
```

## Обработчики команд

### Базовый интерфейс
```csharp
public interface ICommandHandler<in TCommand, TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
```

### Декоратор валидации
```csharp
public class ValidationCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
{
    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        // Валидация команды
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Выполнение команды
        return await _handler.HandleAsync(command, cancellationToken);
    }
}
```

### Пример обработчика
```csharp
public class CreateHotelCommandHandler : ICreateHotelCommandHandler
{
    public async Task<Hotel> HandleAsync(CreateHotelCommand command, CancellationToken cancellationToken = default)
    {
        // Создание доменного объекта (валидация уже пройдена)
        var hotel = new Hotel(
            command.Name,
            command.City,
            command.Country,
            command.Stars,
            command.Rate,
            command.LocationX,
            command.LocationY
        );

        // Здесь будет логика сохранения в базу данных
        
        return hotel;
    }
}
```

## Использование

### Регистрация в DI контейнере
```csharp
// Регистрация валидаторов
services.AddScoped<IValidator<CreateHotelCommand>, CreateHotelCommandValidator>();
services.AddScoped<IValidator<CreateRoomCommand>, CreateRoomCommandValidator>();
services.AddScoped<IValidator<CreateBookingCommand>, CreateBookingCommandValidator>();

// Регистрация обработчиков
services.AddScoped<ICreateHotelCommandHandler, CreateHotelCommandHandler>();

// Регистрация декораторов
services.Decorate<ICreateHotelCommandHandler, ValidationCommandHandlerDecorator<CreateHotelCommand, Hotel>>();
```

### Использование в контроллере
```csharp
[HttpPost]
public async Task<IActionResult> CreateHotel([FromBody] CreateHotelCommand command)
{
    try
    {
        var hotel = await _createHotelCommandHandler.HandleAsync(command);
        return Ok(hotel);
    }
    catch (ValidationException ex)
    {
        return BadRequest(ex.Errors);
    }
}
```

## Преимущества новой архитектуры

1. **Разделение ответственности**: Валидация отделена от доменных объектов
2. **Гибкость валидации**: FluentValidation предоставляет богатые возможности
3. **Переиспользование**: Валидаторы можно использовать в разных местах
4. **Тестируемость**: Легко тестировать валидацию отдельно от бизнес-логики
5. **Логирование**: Автоматическое логирование процесса валидации
6. **Расширяемость**: Легко добавлять новые правила валидации
7. **Чистота домена**: Доменные объекты содержат только бизнес-логику

## Миграция с предыдущей версии

При переходе с валидации в конструкторах:

1. Удалите валидацию из конструкторов доменных объектов
2. Создайте команды для операций создания/изменения
3. Создайте валидаторы FluentValidation для команд
4. Создайте обработчики команд
5. Настройте декораторы для автоматической валидации
6. Обновите контроллеры для использования команд

## Зависимости

Для работы с новой архитектурой потребуются следующие NuGet пакеты:

```xml
<PackageReference Include="FluentValidation" Version="11.x.x" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.x.x" />
<PackageReference Include="Microsoft.Extensions.Logging" Version="8.x.x" />
``` 