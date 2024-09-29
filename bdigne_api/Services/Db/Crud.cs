using System.Linq.Expressions;
using bdigne_api.Db;
using bdigne_api.Utils;
using Microsoft.EntityFrameworkCore;

namespace bdigne_api.Services.Db;

public class Crud<T> : IGenericServiceCrud<T> where T : class
{
    private readonly ApplicationDbContext _db;

    public Crud(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _db.Set<T>().ToListAsync();
    }

    public async Task<Object> GetByIdAsync<TDto>(int id, params Expression<Func<T, object>>[] includes)
    {
        if (includes.Length == 0)
        {
            return await _db.Set<T>().FindAsync(id);
        }
        
        IQueryable<T> query = _db.Set<T>();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        query = query.Where(entity => EF.Property<int>(entity, "Id") == id);
        var res = await query.ToListAsync();

        var dtoMapper = new DtoMapper();
        return dtoMapper.MapToDto<T, TDto>(res.FirstOrDefault());

        // var cycleHandler = new CycleHandler();
        // var cleared = cycleHandler.RemoveCycles(dto);
        // return cleared.FirstOrDefault();
    }

    public async Task<T> CreateAsync(T entity)
    {
        _db.Set<T>().Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}