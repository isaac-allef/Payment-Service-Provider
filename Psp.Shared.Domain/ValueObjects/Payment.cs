namespace Psp.Shared.Domain;

public struct Payment
{
    public PaymentMethod Method { get; }

    // Base card fields
    public CardNumber? CardNumber { get; private set; }
    public string? CardHolderName { get; private set; }
    public Cvv? CardCvv { get; private set; }
    public ExpirationDate? CardExpirationDate { get; private set; }

    // Specific debit card fields
    //...

    // Specific credit card fields
    //...

    // Others payment types
    //...

    private Payment(PaymentMethod method)
    {
        Method = method;
        CardNumber = null;
        CardHolderName = null;
        CardCvv = null;
        CardExpirationDate = null;
    }

    public static Payment NewDebitCardPayment(CardNumber number, string holderName, Cvv cvv, DateTime expirationDate/*, specific debit card fields*/)
        => new Payment(method: PaymentMethod.DEBIT_CARD)
        {
            CardNumber = number,
            CardHolderName = holderName,
            CardCvv = cvv,
            CardExpirationDate = expirationDate
        };

    public static Payment NewCreditCardPayment(CardNumber number, string holderName, Cvv cvv, DateTime expirationDate/*, specific credit card fields*/)
        => new Payment(method: PaymentMethod.CREDIT_CARD)
        {
            CardNumber = number,
            CardHolderName = holderName,
            CardCvv = cvv,
            CardExpirationDate = expirationDate
        };
}
