using Cofoundry.Domain;

namespace AccedeTeams.Domain
{
    public class MemberRole : IRoleDefinition
    {
        public const string MemberRoleCode = "MEM";

        public string Title { get { return "Member"; } }

        public string RoleCode { get { return MemberRoleCode; } }

        public string UserAreaCode { get { return MemberUserArea.MemberUserAreaCode; } }
    }
}
