using Psp.Shared.Domain.Entities;
using Psp.Shared.Models.Requests;
using Psp.Shared.Services.Mq;
using Psp.Worker.Business.Factories;
using Psp.Worker.UseCases;

namespace Psp.Worker;

public class TransactionsListener : BackgroundService
{
    private readonly RabbitMqConfiguration _messageConfiguration;
    private readonly ProcessTransactionHandler _handler;

    public TransactionsListener(RabbitMqConfiguration messageConfiguration, ProcessTransactionHandler handler)
    {
        _messageConfiguration = messageConfiguration;
        _handler = handler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var messageConsumer = RabbitMqAdapter.NewConsumer(_messageConfiguration, queue: "transactions");
        await messageConsumer.ConsumeAsync<TransactionRequest>(execution: async (request) => {
            var transaction = TransactionFactory.Create(request);
            var payable = await _handler.Process(transaction);

            var messageProducer = RabbitMqAdapter.NewProducer(_messageConfiguration, exchange: "process", routingKey: $"{transaction.CustomerId}.payable");
            await messageProducer.PublishAsync<Payable>(payable);
        });
    }
}
