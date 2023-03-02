using System.Text.RegularExpressions;

namespace Psp.Shared.Domain;

public struct Cvv
{
    private const string FORMAT_REGEX = @"^[1-9]{3}$";
    const string ERROR_MESSAGE = "No valid cvv format";
    private string _value { get; set; }

    private Cvv(string value)
    {
        _value = value;
    }

    public static Cvv Parse(string value)
    {
        if (TryParse(value, out var result))
        {
            return result;
        }

        throw new ArgumentException(ERROR_MESSAGE);
    }

    public static bool TryParse(string value, out Cvv cardNumber)
    {
        cardNumber = new Cvv(value);

        if (!Regex.IsMatch(value, FORMAT_REGEX))
        {
            return false;
        }

        return true;
    }

    public override string ToString()
        => _value;

    public static implicit operator Cvv(string input)
        => Parse(input);

    public static implicit operator string(Cvv input)
        => input.ToString();
}
