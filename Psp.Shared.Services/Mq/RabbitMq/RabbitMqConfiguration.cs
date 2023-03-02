using RabbitMQ.Client;

namespace Psp.Shared.Services.Mq;

public class RabbitMqConfiguration
{
    public string ConnectionString { get; private set; }

    public RabbitMqConfiguration(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public IModel ConfigureChannel(IModel channel)
    {
        channel.ExchangeDeclare(exchange: "deadletter", type: ExchangeType.Topic);
        channel.QueueDeclare(
            queue: "transactionsDeadletter",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        channel.QueueDeclare(
            queue: "payablesDeadletter",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        channel.QueueBind(queue: "transactionsDeadletter", exchange: "deadletter", routingKey: "*.transaction", arguments: null);
        channel.QueueBind(queue: "payablesDeadletter", exchange: "deadletter", routingKey: "*.payable", arguments: null);
        var arguments = new Dictionary<string, object>()
        {
            {"x-dead-letter-exchange", "deadletter"}
        };

        channel.ExchangeDeclare(exchange: "process", type: ExchangeType.Topic);

        channel.QueueDeclare(
            queue: "transactions",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: arguments
        );

        channel.QueueDeclare(
            queue: "payables",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: arguments
        );

        channel.QueueBind(queue: "transactions", exchange: "process", routingKey: "*.transaction", arguments: null);
        channel.QueueBind(queue: "payables", exchange: "process", routingKey: "*.payable", arguments: null);

        return channel;
    }
}
