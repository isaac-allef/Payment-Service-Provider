using Psp.Shared.Domain;
using Psp.Shared.Domain.Entities;

namespace Psp.Worker.Business;

public sealed class DebitCardPayable : IMethodCreatePayable, INext<IMethodCreatePayable>
{
    public IMethodCreatePayable? Next { get; set; }

    public Payable Create(Transaction transaction)
    {
        if (!PaymentMethod.DEBIT_CARD.Equals(transaction.Payment.Method))
        {
            return Next is not null ? Next.Create(transaction) 
            : throw new ArgumentNullException(nameof(PaymentMethod.DEBIT_CARD));
        }

        const int DAYS_PLUS = 0;
        const double FEE = 0.03;

        return Payable.New(
            status: PayableStatus.PAID,
            payIn: TimeSpan.FromDays(DAYS_PLUS),
            amount: transaction.Amount - transaction.Amount * FEE,
            transaction: transaction,
            customerId: transaction.CustomerId
        );
    }
}
