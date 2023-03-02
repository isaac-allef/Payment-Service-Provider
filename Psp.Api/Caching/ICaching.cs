namespace Psp.Api.Caching;

public interface ICaching
{
    public Task<TValue?> GetAsync<TValue>(string key);
    public Task SetAsync<TValue>(string key, TValue value, TimeSpan expiration);
}
