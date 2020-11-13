using Cofoundry.Core.Mail;
using Cofoundry.Domain;
using Cofoundry.Domain.CQS;
using Cofoundry.Domain.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccedeTeams.Domain
{
    public class RegisterMemberAndLogInCommandHandler : IAsyncCommandHandler<RegisterMemberAndLogInCommand>, IIgnorePermissionCheckHandler
    {
        private readonly CofoundryDbContext _dbContext;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IUserContextService _userContextService;
        private readonly ILoginService _loginService;
        private readonly IExecutionContextFactory _executionContextFactory;
        private readonly IMailService _mailService;
        
        public RegisterMemberAndLogInCommandHandler(
            CofoundryDbContext dbContext,
            ICommandExecutor commandExecutor,
            IUserContextService userContextService,
            ILoginService loginService,
            IExecutionContextFactory executionContextFactory,
            IMailService mailService
            )
        {
            _dbContext = dbContext;
            _commandExecutor = commandExecutor;
            _userContextService = userContextService;
            _loginService = loginService;
            _executionContextFactory = executionContextFactory;
            _mailService = mailService;
        }

        public async Task ExecuteAsync(RegisterMemberAndLogInCommand command, IExecutionContext executionContext)
        {
            int roleId = await GetMemberRoleId();

            var addUserCommand = MapAddUserCommand(command, roleId);

            await SaveNewUser(executionContext, addUserCommand);
            await SendWelcomeNotification(command);

            await _loginService.LogAuthenticatedUserInAsync(addUserCommand.UserAreaCode, addUserCommand.OutputUserId, true);
        }

        private async Task<int> GetMemberRoleId()
        {
            return await _dbContext
                .Roles
                .AsNoTracking()
                .Where(r => r.RoleCode == MemberRole.MemberRoleCode && r.UserAreaCode == MemberUserArea.MemberUserAreaCode)
                .Select(r => r.RoleId)
                .SingleOrDefaultAsync();
        }

        private async Task SaveNewUser(IExecutionContext executionContext, AddUserCommand addUserCommand)
        {
            var systemExecutionContext = await _executionContextFactory.CreateSystemUserExecutionContextAsync(executionContext);

            await _commandExecutor.ExecuteAsync(addUserCommand, systemExecutionContext);
        }

        private AddUserCommand MapAddUserCommand(RegisterMemberAndLogInCommand command, int roleId)
        {
            var addUserCommand = new AddUserCommand();
            addUserCommand.Email = command.Email;
            addUserCommand.FirstName = command.FirstName;
            addUserCommand.LastName = command.LastName;
            addUserCommand.Password = command.Password;
            addUserCommand.RoleId = roleId;
            addUserCommand.UserAreaCode = MemberUserArea.MemberUserAreaCode;

            return addUserCommand;
        }

        private async Task SendWelcomeNotification(RegisterMemberAndLogInCommand command)
        {
            var welcomeEmailTemplate = new NewUserWelcomeMailTemplate();
            welcomeEmailTemplate.FirstName = command.FirstName;
            await _mailService.SendAsync(command.Email, welcomeEmailTemplate);
        }
    }
}
