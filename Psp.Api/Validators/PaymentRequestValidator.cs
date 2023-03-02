using Psp.Shared.Domain;
using Psp.Shared.Domain.Extensions;
using Psp.Shared.Models.Requests;

namespace Psp.Api.Validators;

public class PaymentRequestValidator
{
    private ValidatorNotification _notification;

    public PaymentRequestValidator(ValidatorNotification notification)
    {
        _notification = notification;
    }

    public void Validate(PaymentRequest request)
    {
        var method = request.Method.ToUpper();

        if (PaymentMethod.DEBIT_CARD.GetDescription().Equals(method))
        {
            if (request.DebitCard is null) _notification.AddError("Debit card fields is void");
            else new BaseCardRequestValidator(_notification).Validate(request.DebitCard);
        }

        else if (PaymentMethod.CREDIT_CARD.GetDescription().Equals(method))
        {
            if (request.CreditCard is null) _notification.AddError("Credit card fields is void");
            else new BaseCardRequestValidator(_notification).Validate(request.CreditCard);
        }

        else _notification.AddError("Wrong payment method");
    }
}
