using Psp.Api.Viewers;
using Psp.Shared.Services.Db;
using Dapper;
using Psp.Api.Caching;

namespace Psp.Api.Db;

public class BalanceFacade
{
    private readonly DbSession _session;
    private readonly ICaching _caching;

    public BalanceFacade(DbSession session, ICaching caching)
    {
        _session = session;
        _caching = caching;
    }

    public async Task<BalanceViewer?> GetManyCacheDecorator(string customerId)
    {
        var key = customerId + nameof(BalanceViewer) + "key";

        var balance = await _caching.GetAsync<BalanceViewer?>(key);
        
        if (balance is not null)
        {
            return balance;
        }

        balance = await GetMany(customerId);

        if (balance is not null)
        {
            await _caching.SetAsync(key, balance,TimeSpan.FromSeconds(30));
        }

        return balance;
    }

    public async Task<BalanceViewer?> GetMany(string customerId)
    {
        var query = $"SELECT * FROM public.\"customers\" WHERE \"id\"=@customerId LIMIT 1";

        return await _session.Connection.QuerySingleOrDefaultAsync<BalanceViewer>(query, new { customerId });
    }
}
