using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Bookings.Domain.Handlers;

/// <summary>
/// Декоратор для валидации команд
/// </summary>
/// <typeparam name="TCommand">Тип команды</typeparam>
/// <typeparam name="TResult">Тип результата</typeparam>
public class ValidationCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _handler;
    private readonly IValidator<TCommand> _validator;
    private readonly ILogger<ValidationCommandHandlerDecorator<TCommand, TResult>> _logger;

    public ValidationCommandHandlerDecorator(
        ICommandHandler<TCommand, TResult> handler,
        IValidator<TCommand> validator,
        ILogger<ValidationCommandHandlerDecorator<TCommand, TResult>> logger)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Валидация команды {CommandType}", typeof(TCommand).Name);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            _logger.LogWarning("Валидация команды {CommandType} не прошла: {Errors}", 
                typeof(TCommand).Name, string.Join(", ", errors));
            
            throw new ValidationException(validationResult.Errors);
        }

        _logger.LogInformation("Валидация команды {CommandType} прошла успешно", typeof(TCommand).Name);

        return await _handler.HandleAsync(command, cancellationToken);
    }
} 