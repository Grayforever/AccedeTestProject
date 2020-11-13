using Cofoundry.Core;
using Cofoundry.Core.AutoUpdate;
using System.Collections.Generic;

namespace AccedeTeams.Domain.Install
{
    public class UpdatePackageFactory : BaseDbOnlyUpdatePackageFactory
    {
        public override string ModuleIdentifier
        {
            get { return "AccedeTeams"; }
        }

        public override ICollection<string> DependentModules { get; } = new string[] { CofoundryModuleInfo.ModuleIdentifier };
    }
}
