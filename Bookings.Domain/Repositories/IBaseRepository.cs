namespace Bookings.Domain.Repositories;

/// <summary>
/// Базовый интерфейс репозитория
/// </summary>
/// <typeparam name="TEntity">Тип доменной сущности</typeparam>
public interface IBaseRepository<TEntity> where TEntity : BaseObject
{
    /// <summary>
    /// Создать сущность
    /// </summary>
    /// <param name="obj">Сущность для создания</param>
    Task Create(TEntity obj);

    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <param name="obj">Сущность для обновления</param>
    void Update(TEntity obj);

    /// <summary>
    /// Удалить сущность по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    void Delete(string id);

    /// <summary>
    /// Получить сущность по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Сущность</returns>
    Task<TEntity> Get(string id);

    /// <summary>
    /// Получить список сущностей с пагинацией
    /// </summary>
    /// <param name="pageNumber">Номер страницы</param>
    /// <param name="pageSize">Размер страницы</param>
    /// <returns>Список сущностей</returns>
    Task<List<TEntity>> Get(int pageNumber, int pageSize);
} 