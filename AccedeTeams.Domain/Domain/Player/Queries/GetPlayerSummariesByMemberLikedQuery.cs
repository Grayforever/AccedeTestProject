using Cofoundry.Domain.CQS;
using System.Collections.Generic;

namespace AccedeTeams.Domain
{
    public class GetPlayerSummariesByMemberLikedQuery : IQuery<ICollection<PlayerSummary>>
    {
        public GetPlayerSummariesByMemberLikedQuery() {}

        public GetPlayerSummariesByMemberLikedQuery(int id)
        {
            UserId = id;
        }

        public int UserId { get; set; }
    }
}
