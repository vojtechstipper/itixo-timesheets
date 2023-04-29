using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Domain;

public class Invoice : IEntity<int>
{
    public int Id { get; set; }
    public string Number { get; set; }

    public static Invoice Create(string number)
    {
        return new Invoice { Number = number };
    }
}
