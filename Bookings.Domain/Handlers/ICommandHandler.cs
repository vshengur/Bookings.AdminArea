namespace Bookings.Domain.Handlers;

/// <summary>
/// Базовый интерфейс для обработчиков команд
/// </summary>
/// <typeparam name="TCommand">Тип команды</typeparam>
/// <typeparam name="TResult">Тип результата</typeparam>
public interface ICommandHandler<in TCommand, TResult>
{
    /// <summary>
    /// Обрабатывает команду
    /// </summary>
    /// <param name="command">Команда для обработки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат обработки команды</returns>
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
} 