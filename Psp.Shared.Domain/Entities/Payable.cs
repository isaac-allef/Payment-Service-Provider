namespace Psp.Shared.Domain.Entities;

public class Payable : IEntity<Guid>
{
    public Guid Id { get; }
    public PayableStatus Status { get; private set; }
    public DateTime PaymentDate { get; }
    public double Amount { get; }
    public Transaction Transaction { get; private set; }
    public string CustomerId { get; private set; }

    public void UpdateStatus(PayableStatus status)
    {
        Status = status;
    }

    public Payable(
        Guid id, PayableStatus status, DateTime paymentDate, double amount, Transaction transaction, string customerId)
    {
        Id = id;
        Status = status;
        PaymentDate = paymentDate;
        Amount = amount;
        Transaction = transaction;
        CustomerId = customerId;
    }

    public static Payable New(PayableStatus status, TimeSpan payIn, double amount, Transaction transaction, string customerId)
        => new Payable(Guid.NewGuid(), status, CalculatePaymentDate(payIn), amount, transaction, customerId);
    
    private static DateTime CalculatePaymentDate(TimeSpan payIn)
        => DateTime.UtcNow + payIn;
}
