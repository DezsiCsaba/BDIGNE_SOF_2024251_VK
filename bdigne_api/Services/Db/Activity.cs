using System.Linq.Expressions;
using bdigne_api.Db;
using bdigne_api.Db.Models;

namespace bdigne_api.Services.Db;

//Teknikailag valszeg csak crud function-ök lesznek, de inkább csináltam neki egy saját service-t, mert lehet lesz nonCrud
//  illetve a controllerekben lévő crud és nonCrud service-ek a saját modeljüket használják nem az activity-t
public class Activity<T> : IGenericServiceCrud<T> where T : class
{
    private readonly ApplicationDbContext _db;

    public Activity(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<object> GetByIdAsync<TDto>(int id, params Expression<Func<T, object>>[] includes)
    {
        throw new NotImplementedException();
    }

    public async Task<T> CreateAsync(T entity)
    {
        _db.Set<T>().Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}