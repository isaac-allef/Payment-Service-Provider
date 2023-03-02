using Psp.Shared.Domain.Entities;

namespace Psp.Shared.Services.Db.Repositories;

public interface ITransactionRepository
{
    public Task Insert(Transaction transaction);
    public Task Attach(Payable payable);
}
