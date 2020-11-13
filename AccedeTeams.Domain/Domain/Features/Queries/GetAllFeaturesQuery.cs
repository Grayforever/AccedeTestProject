using Cofoundry.Domain;
using Cofoundry.Domain.CQS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccedeTeams.Domain
{
    public class GetAllFeaturesQuery : IQuery<ICollection<Feature>>
    {
    }
}
