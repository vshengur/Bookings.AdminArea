# Bookings.Application

Слой приложения (Application Layer) в архитектуре Clean Architecture для системы бронирования.

## Описание

Application слой содержит бизнес-логику приложения, которая не относится к доменной модели, но необходима для координации между различными слоями системы. Этот слой реализует паттерн CQRS (Command Query Responsibility Segregation) с использованием команд и обработчиков.

## Структура проекта

```
Bookings.Application/
├── Commands/                    # Команды для изменения состояния
│   ├── CreateHotelCommand.cs
│   ├── CreateRoomCommand.cs
│   └── CreateBookingCommand.cs
├── Handlers/                    # Обработчики команд
│   ├── ICommandHandler.cs       # Базовый интерфейс
│   ├── ICreateHotelCommandHandler.cs
│   ├── ICreateRoomCommandHandler.cs
│   ├── ICreateBookingCommandHandler.cs
│   ├── ValidationCommandHandlerDecorator.cs
│   └── Implementations/         # Реализации обработчиков
│       ├── CreateHotelCommandHandler.cs
│       ├── CreateRoomCommandHandler.cs
│       └── CreateBookingCommandHandler.cs
├── Validators/                  # Валидаторы команд
│   ├── CreateHotelCommandValidator.cs
│   ├── CreateRoomCommandValidator.cs
│   └── CreateBookingCommandValidator.cs
├── Services/                    # Сервисы приложения
│   ├── Abstractions/           # Интерфейсы сервисов
│   └── Implementations/        # Реализации сервисов
├── Examples/                    # Примеры использования
│   └── ApplicationUsageExample.cs
└── ApplicationModule.cs         # Модуль регистрации сервисов
```

## Основные компоненты

### Команды (Commands)

Команды представляют намерения изменить состояние системы:

- `CreateHotelCommand` - создание нового отеля
- `CreateRoomCommand` - создание нового номера
- `CreateBookingCommand` - создание нового бронирования

### Обработчики (Handlers)

Обработчики содержат бизнес-логику для выполнения команд:

- `ICommandHandler<TCommand, TResult>` - базовый интерфейс
- `ValidationCommandHandlerDecorator<TCommand, TResult>` - декоратор для валидации
- Специализированные обработчики для каждой команды

### Валидаторы (Validators)

Валидаторы проверяют корректность данных команд:

- Используют FluentValidation
- Проверяют бизнес-правила
- Возвращают понятные сообщения об ошибках

## Использование

### Регистрация сервисов

```csharp
// В Program.cs или Startup.cs
services.AddApplicationServices();
```

### Использование в контроллере

```csharp
public class HotelsController : ControllerBase
{
    private readonly ICommandHandler<CreateHotelCommand, Hotel> _handler;

    public HotelsController(ICommandHandler<CreateHotelCommand, Hotel> handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelCommand command)
    {
        try
        {
            var hotel = await _handler.HandleAsync(command);
            return Ok(hotel);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
    }
}
```

## Принципы

1. **Разделение ответственности** - каждый обработчик отвечает за одну команду
2. **Валидация** - все команды проходят валидацию через декораторы
3. **Логирование** - все операции логируются
4. **Обработка ошибок** - четкое разделение ошибок валидации и бизнес-логики
5. **Тестируемость** - все компоненты легко тестируются благодаря интерфейсам

## Зависимости

- `Bookings.Domain` - доменная модель
- `FluentValidation` - валидация
- `Microsoft.Extensions.DependencyInjection` - DI контейнер
- `Microsoft.Extensions.Logging` - логирование
- `Scrutor` - декораторы

## Расширение

Для добавления новой функциональности:

1. Создайте новую команду в папке `Commands/`
2. Создайте валидатор в папке `Validators/`
3. Создайте интерфейс обработчика в папке `Handlers/`
4. Создайте реализацию обработчика в папке `Handlers/Implementations/`
5. Зарегистрируйте сервисы в `ApplicationModule.cs` 