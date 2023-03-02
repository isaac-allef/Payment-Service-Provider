namespace Psp.Shared.Domain;

public struct ExpirationDate
{
    private DateTime _value { get; set; }
    public bool IsExpired { get => _value < DateTime.Today; }

    private ExpirationDate(DateTime value)
    {
        _value = value;
    }

    public DateTime ToDateTime()
        => _value;

    public static implicit operator ExpirationDate(DateTime input)
        => new ExpirationDate(input);

    public static implicit operator DateTime(ExpirationDate input)
        => input.ToDateTime();
}
