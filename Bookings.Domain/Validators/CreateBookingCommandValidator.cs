using Bookings.Domain.Commands;
using FluentValidation;

namespace Bookings.Domain.Validators;

/// <summary>
/// Валидатор для команды создания бронирования
/// </summary>
public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.BookName)
            .NotEmpty()
            .WithMessage("Имя бронирования не может быть пустым")
            .MaximumLength(200)
            .WithMessage("Имя бронирования не может превышать 200 символов");

        RuleFor(x => x.RoomId)
            .NotEmpty()
            .WithMessage("ID номера не может быть пустым")
            .Matches(@"^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$")
            .WithMessage("ID номера должен быть в формате GUID");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Цена должна быть положительной")
            .LessThanOrEqualTo(1000000)
            .WithMessage("Цена не может превышать 1,000,000");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Категория не может быть пустой")
            .MaximumLength(50)
            .WithMessage("Категория не может превышать 50 символов");

        RuleFor(x => x.StateId)
            .NotEqual(Guid.Empty)
            .WithMessage("StateId не может быть пустым");

        RuleFor(x => x.StartDate)
            .NotEqual(DateOnly.MinValue)
            .WithMessage("Дата начала должна быть валидной")
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Дата начала должна быть в будущем");

        RuleFor(x => x.EndDate)
            .NotEqual(DateOnly.MinValue)
            .WithMessage("Дата окончания должна быть валидной")
            .GreaterThan(x => x.StartDate)
            .WithMessage("Дата окончания должна быть позже даты начала");

        RuleFor(x => x.Adults)
            .GreaterThan(0)
            .WithMessage("Количество взрослых должно быть положительным")
            .LessThanOrEqualTo(10)
            .WithMessage("Количество взрослых не может превышать 10");

        RuleFor(x => x.Kids)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество детей не может быть отрицательным")
            .LessThanOrEqualTo(10)
            .WithMessage("Количество детей не может превышать 10");

        // Бизнес-правило: общее количество людей не может превышать 20
        RuleFor(x => x)
            .Must(x => x.Adults + x.Kids <= 20)
            .WithMessage("Общее количество людей не может превышать 20");

        // Бизнес-правило: бронирование не может быть более чем на 365 дней
        RuleFor(x => x)
            .Must(x => (x.EndDate.DayNumber - x.StartDate.DayNumber) <= 365)
            .WithMessage("Бронирование не может быть более чем на 365 дней");
    }
} 