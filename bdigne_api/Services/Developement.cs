using bdigne_api.Db;

namespace bdigne_api.Services;

public class Developement
{
    private readonly ApplicationDbContext _dbContext;

    public Developement(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Object> FullClearAndSeedDb(string queryParam)
    {
        try
        {
            bool force = bool.Parse(queryParam);
            _dbContext.CheckTablesAndRecreateIfNeeded(force: force);
            _dbContext.ClearDatabase();
            _dbContext.Seed();
            return new { success = true };
        }
        catch (Exception e)
        {
            return new {  success = false, message = e.Message};
        }
    }
    
    public async Task<Object> ClearTablesAndSeedDb()
    {
        try
        {
            _dbContext.ClearDatabase();
            _dbContext.Seed();
            return new { success = true };
        }
        catch (Exception e)
        {
            return new {  success = false, message = e.Message};
        }
    }
}