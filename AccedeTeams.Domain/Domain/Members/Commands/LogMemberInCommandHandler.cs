using Cofoundry.Domain;
using Cofoundry.Domain.CQS;
using System.Threading.Tasks;

namespace AccedeTeams.Domain
{
    public class LogMemberInCommandHandler : IAsyncCommandHandler<LogMemberInCommand>, IIgnorePermissionCheckHandler
    {
        private readonly ICommandExecutor _commandExecutor;
        
        public LogMemberInCommandHandler(ICommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        public Task ExecuteAsync(LogMemberInCommand command, IExecutionContext executionContext)
        {
            return _commandExecutor.ExecuteAsync(new LogUserInWithCredentialsCommand()
            {
                Username = command.Email,
                Password = command.Password,
                UserAreaCode = MemberUserArea.MemberUserAreaCode,
                RememberUser = true
            });
        }
    }
}
