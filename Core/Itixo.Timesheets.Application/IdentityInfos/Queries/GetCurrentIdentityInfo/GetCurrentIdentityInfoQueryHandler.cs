using AutoMapper;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.IdentityInfos.Queries.GetCurrentIdentityInfo;

public class GetCurrentIdentityInfoQueryHandler : IRequestHandler<GetCurrentIdentityInfoQuery, GetCurrentIdentityInfoResponse>
{
    private readonly ICurrentIdentityProvider currentIdentityProvider;
    private readonly IPersistenceQuery<IdentityInfo, int> identityInfoQuery;
    private readonly IMapper mapper;

    public GetCurrentIdentityInfoQueryHandler(ICurrentIdentityProvider currentIdentityProvider, IPersistenceQuery<IdentityInfo, int> identityInfoQuery, IMapper mapper)
    {
        this.currentIdentityProvider = currentIdentityProvider;
        this.identityInfoQuery = identityInfoQuery;
        this.mapper = mapper;
    }

    public async Task<GetCurrentIdentityInfoResponse> Handle(GetCurrentIdentityInfoQuery request, CancellationToken cancellationToken)
    {
        IQueryable<IdentityInfo> queryableIdentityInfo = identityInfoQuery.GetQueryable().Where(w => w.ExternalId == currentIdentityProvider.ExternalId);
        GetCurrentIdentityInfoResponse result = await mapper.ProjectTo<GetCurrentIdentityInfoResponse>(queryableIdentityInfo).FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            return new GetCurrentIdentityInfoResponse {DoesIdentityInfoExists = false};
        }

        return result;
    }
}
