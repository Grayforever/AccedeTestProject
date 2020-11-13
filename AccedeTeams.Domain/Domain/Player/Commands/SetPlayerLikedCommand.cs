using Cofoundry.Core.Validation;
using Cofoundry.Domain.CQS;
using System.ComponentModel.DataAnnotations;

namespace AccedeTeams.Domain
{
    public class SetPlayerLikedCommand : ICommand, ILoggableCommand
    {
        [PositiveInteger]
        [Required]
        public int PlayerId { get; set; }

        public bool IsLiked { get; set; }
    }

}
