namespace OnlineCourse.Application.ServiceContracts;

public interface IRedisService
{
    Task SetAsync<T>(string key,T item);
    Task<TResponse?> GetAsync<TResponse>(string key);
}
