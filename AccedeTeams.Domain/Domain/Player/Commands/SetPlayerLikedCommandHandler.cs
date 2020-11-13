using AccedeTeams.Data;
using Cofoundry.Core.EntityFramework;
using Cofoundry.Domain;
using Cofoundry.Domain.CQS;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace AccedeTeams.Domain
{
    public class SetPlayerLikedCommandHandler  : IAsyncCommandHandler<SetPlayerLikedCommand>, ILoggedInPermissionCheckHandler
    {
        private readonly IEntityFrameworkSqlExecutor _entityFrameworkSqlExecutor;
        private readonly AccedeTeamsDbContext _teamsSiteDbContext;

        public SetPlayerLikedCommandHandler(IEntityFrameworkSqlExecutor entityFrameworkSqlExecutor, AccedeTeamsDbContext spaSiteDbContext)      
        {
            _entityFrameworkSqlExecutor = entityFrameworkSqlExecutor;
            _teamsSiteDbContext = spaSiteDbContext;
        }

        public Task ExecuteAsync(SetPlayerLikedCommand command, IExecutionContext executionContext)
        {
            return _entityFrameworkSqlExecutor
                .ExecuteCommandAsync(_teamsSiteDbContext,
                "app.PlayerLike_SetLiked",
                 new SqlParameter("@PlayerId", command.PlayerId),
                 new SqlParameter("@UserId", executionContext.UserContext.UserId),
                 new SqlParameter("@IsLiked", command.IsLiked),
                 new SqlParameter("@CreateDate", executionContext.ExecutionDate)
                 );
        }
    }

}
