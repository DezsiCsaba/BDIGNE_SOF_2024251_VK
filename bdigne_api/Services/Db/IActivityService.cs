namespace bdigne_api.Services.Db;

public interface IActivityService<T> where T: class
{
    Task<T> CreateActivity(T activity);
}