using System.Linq.Expressions;

namespace bdigne_api.Services.Db;

public interface IGenericService<T> where T : class
{
    Task<T> FindWithOptions(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> FindAllWithOption(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> FindAllWithOptionAndInclude(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes
    );

}