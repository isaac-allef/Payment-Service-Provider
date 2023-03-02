namespace Psp.Shared.Domain.Entities;

public interface IEntity<T>
{
    T Id { get; }
}
