namespace Psp.Shared.Domain.Entities;

public sealed class Customer : IEntity<string>
{
    public string Id { get; private set; }
    public Balance Balance { get; private set; }
    public DateTime UpdatedAt { get; set; }

    public Customer(string id)
    {
        Id = id;
    }

    public void UpdateBalance(Balance newBalance)
    {
        Balance = newBalance;
    }
}
