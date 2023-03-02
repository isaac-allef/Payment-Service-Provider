using Newtonsoft.Json;

namespace Psp.Shared.Models.Requests;

public sealed class PaymentRequest
{
    [JsonProperty("method")]
    public string Method { get; set; } = string.Empty;
    
    [JsonProperty("debit_card")]
    public DebitCardRequest? DebitCard { get; set; }
    
    [JsonProperty("credit_card")]
    public CreditCardRequest? CreditCard { get; set; }
}
