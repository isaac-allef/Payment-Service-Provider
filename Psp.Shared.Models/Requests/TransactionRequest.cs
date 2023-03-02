using Newtonsoft.Json;

namespace Psp.Shared.Models.Requests;

public sealed class TransactionRequest
{
    [JsonProperty("amount")]
    public double Amount { get; set; }
    
    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("payment")]
    public PaymentRequest Payment { get; set; } = new();

    [JsonProperty("customer_id")]
    public string CustomerId { get; set; } = string.Empty;
}
