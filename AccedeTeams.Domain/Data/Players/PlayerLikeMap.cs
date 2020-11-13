using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Cofoundry.Core;

namespace AccedeTeams.Data
{
    public class PlayerLikeMap : IEntityTypeConfiguration<PlayerLike>
    {
        public void Configure(EntityTypeBuilder<PlayerLike> builder)
        {
            builder.ToTable("PlayerLike", DbConstants.DefaultAppSchema);

            builder.HasKey(e => new { e.PlayerCustomEntityId, e.UserId });

            // Relationships
            builder.HasOne(s => s.PlayerCustomEntity)
                .WithMany()
                .HasForeignKey(d => d.PlayerCustomEntityId);

            builder.HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);
        }
    }
}
