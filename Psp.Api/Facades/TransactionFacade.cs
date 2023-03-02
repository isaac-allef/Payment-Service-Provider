using Psp.Api.Viewers;
using Psp.Shared.Services.Db;
using Dapper;
using Psp.Api.Caching;
using Psp.Shared.Models.Requests;
using Psp.Shared.Services.Mq;

namespace Psp.Api.Db;

public class TransactionFacade
{
    private readonly RabbitMqConfiguration _messageConfiguration;
    private readonly DbSession _session;
    private readonly ICaching _caching;

    public TransactionFacade(RabbitMqConfiguration messageConfiguration, DbSession session, ICaching caching)
    {
        _messageConfiguration = messageConfiguration;
        _session = session;
        _caching = caching;
    }

    public async Task PublishTransaction(TransactionRequest request)
    {
        var messageProducer = RabbitMqAdapter.NewProducer(_messageConfiguration, exchange: "process", routingKey: $"{request.CustomerId}.transaction");
        await messageProducer.PublishAsync<TransactionRequest>(request);
        messageProducer.Dispose();
    }

    public async Task<IList<TransactionViewer>> GetManyCacheDecorator(string customerId, int page)
    {
        var key = customerId + page + nameof(TransactionViewer) + "key";

        var transacitons = await _caching.GetAsync<IList<TransactionViewer>>(key);
        
        if (transacitons is not null)
        {
            return transacitons;
        }
        
        transacitons = await GetMany(customerId, page);

        if (transacitons.Count != 0)
        {
            await _caching.SetAsync(key, transacitons, TimeSpan.FromSeconds(30));
        }

        return transacitons;
    }

    public async Task<IList<TransactionViewer>> GetMany(string customerId, int page)
    {            
        const int LIMIT = 5;
        var offset = (page-1) * LIMIT;
        var query = $"SELECT * FROM public.\"transactions\" WHERE \"customer_id\"=@customerId ORDER BY \"created_at\" DESC LIMIT {LIMIT} OFFSET {offset}";

        return (await _session.Connection.QueryAsync<TransactionViewer>(query, new { customerId })).ToList();
    }
}
