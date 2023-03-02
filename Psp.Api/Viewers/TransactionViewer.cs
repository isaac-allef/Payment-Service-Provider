namespace Psp.Api.Viewers;

public struct TransactionViewer
{
    public Guid id { get; set; }
    public double amount { get; set; }
    public string description { get; set; }
    public int payment_method { get; set; }
    public string card_number { get; set; }
    public string card_holder_name { get; set; }
    public string card_cvv { get; set; }   
    public DateTime card_expiration_date { get; set; }
    public DateTime created_at { get; set; }
}
