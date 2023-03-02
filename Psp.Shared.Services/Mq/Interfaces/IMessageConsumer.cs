namespace Psp.Shared.Services.Mq;

public interface IMessageConsumer : IDisposable
{
    public Task ConsumeAsync<TValue>(Func<TValue, Task> execution) where TValue : notnull;
}
