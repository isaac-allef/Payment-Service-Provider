using Psp.Shared.Domain.Entities;
using Psp.Shared.Services.Mq;
using Psp.Worker.UseCases;

namespace Psp.Worker;

public sealed class PayablesListener : BackgroundService
{
    private readonly RabbitMqConfiguration _messageConfiguration;
    
    private readonly UpdateCustomerBalanceHandler _handler;

    public PayablesListener(RabbitMqConfiguration messageConfiguration, UpdateCustomerBalanceHandler handler)
    {
        _messageConfiguration = messageConfiguration;
        _handler = handler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var messageConsumer = RabbitMqAdapter.NewConsumer(_messageConfiguration, queue: "payables");
        await messageConsumer.ConsumeAsync<Payable>(async (payable) => {
                await _handler.Update(payable);
        });
    }
}
