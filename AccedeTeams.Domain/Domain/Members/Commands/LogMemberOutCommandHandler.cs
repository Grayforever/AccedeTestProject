using Cofoundry.Core.Validation;
using Cofoundry.Domain;
using Cofoundry.Domain.CQS;
using Cofoundry.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccedeTeams.Domain
{
    public class LogMemberOutCommandHandler : IAsyncCommandHandler<LogMemberOutCommand>, IIgnorePermissionCheckHandler
    {
        private readonly ILoginService _loginService;
        
        public LogMemberOutCommandHandler(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public Task ExecuteAsync(LogMemberOutCommand command, IExecutionContext executionContext)
        {
            return _loginService.SignOutAsync(MemberUserArea.MemberUserAreaCode);
        }
    }
}
