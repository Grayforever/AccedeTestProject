using Cofoundry.Domain;
using Cofoundry.Domain.CQS;

namespace AccedeTeams.Domain
{
    public class SearchPlayerSummariesQuery
        : SimplePageableQuery
        , IQuery<PagedQueryResult<PlayerSummary>>
    {
    }
}
