using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Psp.Shared.Services.Mq;

public class RabbitMqAdapter : IMessageProducer, IMessageConsumer
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private string? Exchange;
    private string? RoutingKey;
    private string? Queue;

    private RabbitMqAdapter(RabbitMqConfiguration configuration)
    {
        _connectionFactory = new ConnectionFactory()
        {
            Uri = new Uri(configuration.ConnectionString)
        };

        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel = configuration.ConfigureChannel(_channel);

        Exchange = null;
        RoutingKey = null;
        Queue = null;
    }

    public static IMessageProducer NewProducer(RabbitMqConfiguration configuration, string exchange, string routingKey)
        => new RabbitMqAdapter(configuration)
        {
            Exchange = exchange,
            RoutingKey = routingKey
        };

    public static IMessageConsumer NewConsumer(RabbitMqConfiguration configuration, string queue)
        => new RabbitMqAdapter(configuration)
        {
            Queue = queue
        };

    public async Task PublishAsync<TValue>(TValue value) where TValue : notnull
    {
        var message = JsonConvert.SerializeObject(value);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(
            Exchange,
            RoutingKey,
            basicProperties: null,
            body
        );

        await Task.CompletedTask;
    }

    public async Task ConsumeAsync<TValue>(Func<TValue, Task> execution) where TValue : notnull
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var value = JsonConvert.DeserializeObject<TValue>(message);

                await execution(value!);
                
                _channel.BasicAck(
                    deliveryTag: eventArgs.DeliveryTag, multiple: false);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                
                _channel.BasicNack(
                    deliveryTag: eventArgs.DeliveryTag, multiple: false, requeue: false);
            }
        };

        _channel.BasicConsume(
            Queue,
            autoAck: false,
            consumer
        );

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
        }
        if (_connection.IsOpen)
        {
            _connection.Close();
        }
    }
}