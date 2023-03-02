using Psp.Shared.Domain.Entities;

namespace Psp.Worker.Business;

public sealed class CreatePayable
{
    public Payable Create(Transaction transaction)
    {
        var debit = new DebitCardPayable();
        var credit = new CreditCardPayable();
        var no = new NoPayable();

        debit.Next = credit;
        credit.Next = no;

        return debit.Create(transaction);
    }
}
