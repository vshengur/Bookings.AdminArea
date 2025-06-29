using Bookings.Application.Commands;
using FluentValidation;

namespace Bookings.Application.Validators;

/// <summary>
/// Валидатор для команды создания номера
/// </summary>
public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название номера не может быть пустым")
            .MaximumLength(100)
            .WithMessage("Название номера не может превышать 100 символов");

        RuleFor(x => x.MaxPersons)
            .GreaterThan(0)
            .WithMessage("Максимальное количество человек должно быть положительным")
            .LessThanOrEqualTo(20)
            .WithMessage("Максимальное количество человек не может превышать 20");

        RuleFor(x => x.AdditionalFreeKids)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Количество дополнительных бесплатных детей не может быть отрицательным")
            .LessThanOrEqualTo(10)
            .WithMessage("Количество дополнительных бесплатных детей не может превышать 10");

        RuleFor(x => x.HotelId)
            .NotEmpty()
            .WithMessage("ID отеля не может быть пустым")
            .Matches(@"^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$")
            .WithMessage("ID отеля должен быть в формате GUID");

        // Бизнес-правило: количество бесплатных детей не может превышать максимальное количество человек
        RuleFor(x => x)
            .Must(x => x.AdditionalFreeKids <= x.MaxPersons)
            .WithMessage("Количество дополнительных бесплатных детей не может превышать максимальное количество человек");
    }
} 