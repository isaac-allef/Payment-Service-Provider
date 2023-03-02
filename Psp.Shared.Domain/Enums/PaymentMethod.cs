using System.ComponentModel;

namespace Psp.Shared.Domain;

public enum PaymentMethod
{
    [Description("DEBIT_CARD")]
    DEBIT_CARD,
    
    [Description("CREDIT_CARD")]
    CREDIT_CARD
}
