using Psp.Shared.Domain;
using Psp.Shared.Domain.Entities;

namespace Psp.Api.Viewers;

public struct CustomerViewer
{
    public string id { get; set; }
    public double available_amount { get; set; }
    public double waiting_funds_amount { get; set; }
    public DateTime updated_at { get; set; }

    public static implicit operator Customer(CustomerViewer viewer)
    {
        var customer = new Customer(viewer.id);
        customer.UpdateBalance(Balance.New(viewer.available_amount, viewer.waiting_funds_amount));
        customer.UpdatedAt = viewer.updated_at;
        return customer;
    }
}
