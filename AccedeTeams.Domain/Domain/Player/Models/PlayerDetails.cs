using Cofoundry.Domain;
using System.Collections.Generic;

namespace AccedeTeams.Domain
{
    public class PlayerDetails
    {
        public int PlayerId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TotalLikes { get; set; }

        public ICollection<Feature> Features { get; set; }

        public ICollection<ImageAssetRenderDetails> Images { get; set; }
    }
}
