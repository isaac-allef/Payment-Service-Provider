using System.Text.RegularExpressions;

namespace Psp.Shared.Domain;

public struct CardNumber
{
    const string ERROR_MESSAGE = "No valid card number";
    private string _value { get; set; }

    private CardNumber(string value)
    {
        _value = value;
    }

    public static CardNumber Parse(string value)
    {
        if (TryParse(value, out var result))
        {
            return result;
        }

        throw new ArgumentException(ERROR_MESSAGE);
    }

    public static bool TryParse(string value, out CardNumber cardNumber)
    {
        cardNumber = new CardNumber(value);

        if (!LuhnAlgoCheck(value))
        {
            return false;
        }

        return true;
    }

    private static bool LuhnAlgoCheck(string input)
    {
        int[] cardInt = new int[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            cardInt[i] = (int)(input[i] - '0');
        }

        for (int i = cardInt.Length - 2; i>= 0; i = i -2)
        {
            int tempValue = cardInt[i];
            tempValue = tempValue * 2;
            if (tempValue > 9)
            {
                tempValue = tempValue % 10 + 1;
            }
            cardInt[i] = tempValue;
        }

        int total = 0;
        for (int i = 0; i < cardInt.Length; i++)
        {
            total = cardInt[i];
        }

        if (total % 10 == 0)
        {
            return true;
        }

        return false;
    }

    public override string ToString()
        => _value;

    public static implicit operator CardNumber(string input)
        => Parse(input);

    public static implicit operator string(CardNumber input)
        => input.ToString();
    
    public CardNumber Mask()
    {
        var numberOfCharactersMasked = _value.Length - 4;
        var replacementString = new string('*', numberOfCharactersMasked);
        var maskedValue = Regex.Replace(_value, @"^.{" + numberOfCharactersMasked + "}", replacementString);
        return new CardNumber(maskedValue);
    }
}
