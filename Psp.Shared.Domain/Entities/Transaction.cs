namespace Psp.Shared.Domain.Entities;

public sealed class Transaction : IEntity<Guid>
{
    public Guid Id { get; }
    public double Amount { get; }
    public string? Description { get; }
    public Payment Payment { get; }
    public DateTime CreatedAt { get; }
    public string CustomerId { get; }

    public Transaction(Guid id, double amount, string? description, Payment payment, DateTime createdAt, string customerId)
    {
        Id = id;
        Amount = amount;
        Description = description;
        Payment = payment;
        CreatedAt = createdAt;
        CustomerId = customerId;

        Validate();
    }

    private void Validate()
    {
        if (Amount < 0) throw new ArgumentOutOfRangeException(nameof(Amount));
        if (string.IsNullOrWhiteSpace(CustomerId)) throw new ArgumentNullException(nameof(CustomerId));
    }
}
