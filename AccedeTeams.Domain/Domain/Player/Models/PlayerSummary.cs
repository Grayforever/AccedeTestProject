using Cofoundry.Domain;

namespace AccedeTeams.Domain
{
    public class PlayerSummary
    {
        public int PlayerId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TotalLikes { get; set; }

        public ImageAssetRenderDetails MainImage { get; set; }
    }
}
