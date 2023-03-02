using Newtonsoft.Json;

namespace Psp.Shared.Models.Requests;

public abstract class BaseCardRequest
{
    [JsonProperty("number")]
    public string Number { get; set; } = string.Empty;
    
    [JsonProperty("holder_name")]
    public string HolderName { get; set; } = string.Empty;
    
    [JsonProperty("cvv")]
    public string Cvv { get; set; } = string.Empty;
    
    [JsonProperty("expiration_date")]
    public DateTime ExpirationDate { get; set; }
}
