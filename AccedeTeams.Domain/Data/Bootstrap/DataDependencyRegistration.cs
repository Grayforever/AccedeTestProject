using Cofoundry.Core.DependencyInjection;

namespace AccedeTeams.Data
{
    public class DataDependencyRegistration : IDependencyRegistration
    {
        public void Register(IContainerRegister container)
        {
            container.RegisterScoped<AccedeTeamsDbContext>();
        }
    }
}
