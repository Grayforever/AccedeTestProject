using Cofoundry.Domain;
using Cofoundry.Domain.CQS;
using System.Threading.Tasks;

namespace AccedeTeams.Domain
{
    public class GetCurrentMemberSummaryQueryHandler : IAsyncQueryHandler<GetCurrentMemberSummaryQuery, MemberSummary>, IIgnorePermissionCheckHandler
    {
        private readonly IUserRepository _userRepository;

        public GetCurrentMemberSummaryQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<MemberSummary> ExecuteAsync(GetCurrentMemberSummaryQuery query, IExecutionContext executionContext)
        {
            if (!IsLoggedInMember(executionContext.UserContext)) return null;

            var user = await _userRepository.GetCurrentUserMicroSummaryAsync();

            return new MemberSummary()
            {
                UserId = user.UserId,
                Name = user.GetFullName()
            };
        }

        private bool IsLoggedInMember(IUserContext userContext)
        {
            return userContext.UserId.HasValue && userContext.UserArea.UserAreaCode == MemberUserArea.MemberUserAreaCode;
        }
    }
}
