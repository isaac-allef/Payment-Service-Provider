using Dapper;
using Psp.Api.Viewers;
using Psp.Shared.Domain.Entities;

namespace Psp.Shared.Services.Db.Repositories;

public sealed class CustomerPostgresqlRepository : ICustomerRepository
{
    private readonly DbSession _session;

    public CustomerPostgresqlRepository(DbSession session)
    {
        _session = session;
    }

    public async Task<Customer?> GetById(string id)
    {
        var query = $"SELECT id, available_amount, waiting_funds_amount, updated_at FROM public.\"customers\" WHERE \"id\"=@id LIMIT 1";
        
        return await _session.Connection.QuerySingleOrDefaultAsync<CustomerViewer>(query, new { id }, _session.Transaction);
    }

    public async Task<bool> UpdateBalanceIfUpToDate(Customer customer)
    {
        var availableAmount = customer.Balance.AvailableAmount;
        var waitingFundsAmount = customer.Balance.WaitingFundsAmount;
        var id = customer.Id;
        var updatedAt = customer.UpdatedAt;
        
        var query = $"UPDATE ONLY public.\"customers\" SET \"available_amount\"=@availableAmount, \"waiting_funds_amount\"=@waitingFundsAmount, \"updated_at\"=now() at time zone ('utc') WHERE \"id\"=@id AND \"updated_at\"=@updatedAt";

        var linesUpdated = await _session.Connection.ExecuteAsync(
            query,
            new { availableAmount, waitingFundsAmount, id, updatedAt },
            _session.Transaction
        );
        
        return linesUpdated > 0;
    }
}
