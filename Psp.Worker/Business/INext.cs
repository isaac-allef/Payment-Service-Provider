namespace Psp.Worker.Business;

public interface INext<T>
{
    public T? Next { get; set; }
}
