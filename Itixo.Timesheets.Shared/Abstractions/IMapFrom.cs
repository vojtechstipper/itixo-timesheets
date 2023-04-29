using AutoMapper;

namespace Itixo.Timesheets.Shared.Abstractions;

public interface IMapFrom<T>
{
    void Mapping(Profile profile);
}
