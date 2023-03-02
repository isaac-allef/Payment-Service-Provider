using Psp.Shared.Domain;
using Psp.Shared.Models.Requests;

namespace Psp.Api.Validators;

public class BaseCardRequestValidator
{
    private ValidatorNotification _notification;

    public BaseCardRequestValidator(ValidatorNotification notification)
    {
        _notification = notification;
    }

    public void Validate(BaseCardRequest request)
    {
        if (!CardNumber.TryParse(request.Number, out var cardNumber))
            _notification.AddError("Wrong card number");

        if (string.IsNullOrWhiteSpace(request.HolderName))
            _notification.AddError("Holder name can't is void");

        if (!Cvv.TryParse(request.Cvv, out var cvv))
            _notification.AddError("Wrong cvv");

        if (((ExpirationDate)request.ExpirationDate).IsExpired)
            _notification.AddError("Card is expired");
    }
}
