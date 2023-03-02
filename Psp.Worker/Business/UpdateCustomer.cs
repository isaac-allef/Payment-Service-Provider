using Psp.Shared.Domain;
using Psp.Shared.Domain.Entities;

namespace Psp.Worker.Business;

public sealed class UpdateCustomer
{
    private Customer Customer { get; set; }

    public UpdateCustomer(Customer customer)
    {
        Customer = customer;
    }

    public Customer UpdateBalance(Payable payable)
    {
        var availableAmount = Customer.Balance.AvailableAmount;
        var waitingFundsAmount = Customer.Balance.WaitingFundsAmount;

        if (PayableStatus.PAID.Equals(payable.Status))
        {
            availableAmount += payable.Amount;
        }
        else if (PayableStatus.WAITING_FUNDS.Equals(payable.Status))
        {
            waitingFundsAmount += payable.Amount;
        }
        else
        {
            throw new ArgumentNullException(nameof(payable.Status));
        }

        Customer.UpdateBalance(Balance.New(availableAmount, waitingFundsAmount));
        return Customer;

    }
}
