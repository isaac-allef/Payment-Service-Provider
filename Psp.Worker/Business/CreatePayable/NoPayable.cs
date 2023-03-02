using Psp.Shared.Domain;
using Psp.Shared.Domain.Entities;

namespace Psp.Worker.Business;

public sealed class NoPayable : IMethodCreatePayable, INext<IMethodCreatePayable>
{
    public IMethodCreatePayable? Next { get; set; }

    public Payable Create(Transaction transaction)
    {
        throw new ArgumentOutOfRangeException(nameof(PaymentMethod));
    }
}
