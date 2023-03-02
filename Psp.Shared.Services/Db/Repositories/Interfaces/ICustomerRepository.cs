using Psp.Shared.Domain.Entities;

namespace Psp.Shared.Services.Db.Repositories;

public interface ICustomerRepository
{
    public Task<Customer?> GetById(string id);
    public Task<bool> UpdateBalanceIfUpToDate(Customer customer);
}
