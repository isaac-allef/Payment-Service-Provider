namespace Psp.Shared.Services.Mq;

public interface IMessageProducer : IDisposable
{
    public Task PublishAsync<TValue>(TValue value) where TValue : notnull;
}
