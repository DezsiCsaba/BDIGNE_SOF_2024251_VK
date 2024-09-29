using System.Linq.Expressions;
using bdigne_api.Db;
using bdigne_api.Utils;
using Microsoft.EntityFrameworkCore;

namespace bdigne_api.Services.Db;

public class NonCrud<T> : IGenericService<T> where T : class
{
    private readonly ApplicationDbContext _context;

    
    public NonCrud(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<T> FindWithOptions(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().FirstOrDefault(predicate);
    }

    public async Task<IEnumerable<T>> FindAllWithOption(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>()
            .Where(predicate)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<T>> FindAllWithOptionAndInclude(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes
    ){
        IQueryable<T> query = _context.Set<T>().Where(predicate);
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        var res = await query.ToListAsync();

        var cycleHandler = new CycleHandler();
        // res.RemoveAt(1);
        var clearedResult = cycleHandler.RemoveCycles(res);
        return clearedResult;
    }
}