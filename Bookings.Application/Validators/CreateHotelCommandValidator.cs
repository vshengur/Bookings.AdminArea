using Bookings.Application.Commands;
using FluentValidation;

namespace Bookings.Application.Validators;

/// <summary>
/// Валидатор для команды создания отеля
/// </summary>
public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название отеля не может быть пустым")
            .MaximumLength(100)
            .WithMessage("Название отеля не может превышать 100 символов");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("Город не может быть пустым")
            .MaximumLength(50)
            .WithMessage("Название города не может превышать 50 символов");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Страна не может быть пустой")
            .MaximumLength(50)
            .WithMessage("Название страны не может превышать 50 символов");

        RuleFor(x => x.Stars)
            .InclusiveBetween(1, 5)
            .WithMessage("Количество звезд должно быть от 1 до 5");

        RuleFor(x => x.Rate)
            .InclusiveBetween(0, 10)
            .WithMessage("Рейтинг должен быть от 0 до 10");

        RuleFor(x => x.LocationX)
            .InclusiveBetween(-180, 180)
            .WithMessage("Долгота должна быть в диапазоне от -180 до 180");

        RuleFor(x => x.LocationY)
            .InclusiveBetween(-90, 90)
            .WithMessage("Широта должна быть в диапазоне от -90 до 90");
    }
} 