namespace Itixo.Timesheets.Shared.Abstractions;

public interface IEntity<T>
{
    public T Id { get; set; }
}