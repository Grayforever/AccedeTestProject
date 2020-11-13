using Cofoundry.Domain;
using System.Collections.Generic;

namespace AccedeTeams.Domain
{
    public class MemberRoleInitializer : IRoleInitializer<MemberRole>
    {
        public IEnumerable<IPermission> GetPermissions(IEnumerable<IPermission> allPermissions)
        {
            return allPermissions.FilterToAnonymousRoleDefaults();
        }
    }
}
