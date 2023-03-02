using Psp.Shared.Domain.Entities;

namespace Psp.Worker.Business;

public interface IMethodCreatePayable
{
    public Payable Create(Transaction transaction);
}
