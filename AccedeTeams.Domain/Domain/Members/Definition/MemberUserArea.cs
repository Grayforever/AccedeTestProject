using Cofoundry.Domain;

namespace AccedeTeams.Domain
{
    public class MemberUserArea : IUserAreaDefinition
    {
        public const string MemberUserAreaCode = "ACT";

        public bool AllowPasswordLogin => true;

        public string Name => "Accede Team";

        public bool UseEmailAsUsername => true;

        public string UserAreaCode => MemberUserAreaCode;

        public string LoginPath => "/";

        public bool IsDefaultAuthSchema => true;
    }
}
