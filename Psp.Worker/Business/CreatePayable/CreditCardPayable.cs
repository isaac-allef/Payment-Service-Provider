using Psp.Shared.Domain;
using Psp.Shared.Domain.Entities;

namespace Psp.Worker.Business;

public sealed class CreditCardPayable : IMethodCreatePayable, INext<IMethodCreatePayable>
{
    public IMethodCreatePayable? Next { get; set; }

    public Payable Create(Transaction transaction)
    {
        if (!PaymentMethod.CREDIT_CARD.Equals(transaction.Payment.Method))
        {
            return Next is not null ? Next.Create(transaction) 
            : throw new ArgumentNullException(nameof(PaymentMethod.CREDIT_CARD));
        }

        const int DAYS_PLUS = 30;
        const double FEE = 0.05;

        return Payable.New(
            status: PayableStatus.WAITING_FUNDS,
            payIn: TimeSpan.FromDays(DAYS_PLUS),
            amount: transaction.Amount - transaction.Amount * FEE,
            transaction: transaction,
            customerId: transaction.CustomerId
        );
    }
}
