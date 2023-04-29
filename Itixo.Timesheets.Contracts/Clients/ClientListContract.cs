using AutoMapper;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Contracts.Clients;

public class ClientListContract : IMapFrom<Client>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Client, ClientListContract>();
    }
}
