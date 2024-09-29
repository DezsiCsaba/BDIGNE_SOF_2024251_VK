using System.Linq.Expressions;

namespace bdigne_api.Services.Db;

public interface IGenericServiceCrud<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<Object> GetByIdAsync<TDto>(int id, params Expression<Func<T, object>>[] includes);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}