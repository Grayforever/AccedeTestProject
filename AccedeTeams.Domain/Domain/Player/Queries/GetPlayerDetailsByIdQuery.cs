using Cofoundry.Domain.CQS;

namespace AccedeTeams.Domain
{
    public class GetPlayerDetailsByIdQuery : IQuery<PlayerDetails>
    {
        public GetPlayerDetailsByIdQuery() {}

        public GetPlayerDetailsByIdQuery(int id)
        {
            PlayerId = id;
        }

        public int PlayerId { get; set; }
    }
}
