using Dapper;
using Psp.Shared.Domain.Entities;

namespace Psp.Shared.Services.Db.Repositories;

public sealed class TransactionPostgresqlRepository : ITransactionRepository
{
    private readonly DbSession _session;

    public TransactionPostgresqlRepository(DbSession session)
    {
        _session = session;
    }

    public async Task Insert(Transaction transaction)
    {
        var id = transaction.Id;
        var amount = transaction.Amount;
        var description = transaction.Description;
        var paymentMethod = transaction.Payment.Method;
        var cardNumber = transaction.Payment.CardNumber?.Mask().ToString();
        var cardHolderName = transaction.Payment.CardHolderName;
        var cardCvv = transaction.Payment.CardCvv?.ToString();
        var cardExpirationDate = transaction.Payment.CardExpirationDate?.ToDateTime();
        var createdAt = transaction.CreatedAt;
        var customerId = transaction.CustomerId;

        var query = $"INSERT INTO public.\"transactions\" (id, amount, description, payment_method, card_number, card_holder_name, card_cvv, card_expiration_date, created_at, customer_id) VALUES (@id, @amount, @description, @paymentMethod, @cardNumber, @cardHolderName, @cardCvv, @cardExpirationDate, @createdAt, @customerId)";
        var linesUpdated = await _session.Connection.ExecuteAsync(
            query,
            new { id, amount, description, paymentMethod, cardNumber, cardHolderName, cardCvv, cardExpirationDate, createdAt, customerId },
            _session.Transaction
        );
    }
    
    public async Task Attach(Payable payable)
    {
        var id = payable.Id;
        var status = payable.Status;
        var paymentDate = payable.PaymentDate;
        var amount = payable.Amount;
        var transactionId = payable.Transaction.Id;
        var customerId = payable.CustomerId;

        var query = $"INSERT INTO public.\"payables\" (id, status, payment_date, amount, transaction_id, customer_id) VALUES (@id, @status, @paymentDate, @amount, @transactionId, @customerId)";

        var linesUpdated = await _session.Connection.ExecuteAsync(
            query,
            new { id, status, paymentDate, amount, transactionId, customerId },
            _session.Transaction
        );
    }
}
