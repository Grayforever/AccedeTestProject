using Cofoundry.Domain.Data;
using System;

namespace AccedeTeams.Data
{
    public class PlayerLike
    {
        public int PlayerCustomEntityId { get; set; }

        public int UserId { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual User User { get; set; }

        public virtual CustomEntity PlayerCustomEntity { get; set; }
    }
}
