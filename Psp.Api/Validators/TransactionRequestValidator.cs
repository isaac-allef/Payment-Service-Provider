using Psp.Shared.Models.Requests;

namespace Psp.Api.Validators;

public sealed class TransactionRequestValidator
{
    private ValidatorNotification _notification;

    public TransactionRequestValidator(ValidatorNotification notification)
    {
        _notification = notification;
    }

    public void Validate(TransactionRequest request)
    {   
        if (request.Amount <= 0)
            _notification.AddError("Amount need be a positive value");

        if (string.IsNullOrWhiteSpace(request.CustomerId))
            _notification.AddError("CustomerId can't be void");

        new PaymentRequestValidator(_notification).Validate(request.Payment);
    }
}
