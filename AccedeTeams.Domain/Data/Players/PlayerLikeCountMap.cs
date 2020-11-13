using Cofoundry.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccedeTeams.Data
{
    public class PlayerLikeCountMap : IEntityTypeConfiguration<PlayerLikeCount>
    {
        public void Configure(EntityTypeBuilder<PlayerLikeCount> builder)
        {
            builder.ToTable("PlayerLikeCount", DbConstants.DefaultAppSchema);

            builder.HasKey(e => e.PlayerCustomEntityId);
        }
    }
}
