using Psp.Shared.Models.Requests;
using Psp.Shared.Domain.Entities;
using Psp.Shared.Domain.Extensions;
using Psp.Shared.Domain;

namespace Psp.Worker.Business.Factories;

public static class TransactionFactory
{
    public static Transaction Create(TransactionRequest request)
    {
        return new Transaction(
            id: Guid.NewGuid(),
            request.Amount,
            request.Description,
            NewPayment(request.Payment),
            createdAt: DateTime.UtcNow,
            request.CustomerId
        );
    }

    private static Payment NewPayment(PaymentRequest request)
    {
        var method = request.Method.ToUpper();

        if (PaymentMethod.DEBIT_CARD.GetDescription().Equals(method))
        {
            var debitCardRequest = request.DebitCard ?? throw new ArgumentNullException(nameof(request.DebitCard));
            return Payment.NewDebitCardPayment(
                debitCardRequest.Number,
                debitCardRequest.HolderName,
                debitCardRequest.Cvv,
                debitCardRequest.ExpirationDate
            );
        }

        else if (PaymentMethod.CREDIT_CARD.GetDescription().Equals(method))
        {
            var creditCardRequest = request.CreditCard ?? throw new ArgumentNullException(nameof(request.CreditCard));
            return Payment.NewCreditCardPayment(
                creditCardRequest.Number,
                creditCardRequest.HolderName,
                creditCardRequest.Cvv,
                creditCardRequest.ExpirationDate
            );
        }

        throw new ArgumentOutOfRangeException(nameof(PaymentMethod));
    }
}
