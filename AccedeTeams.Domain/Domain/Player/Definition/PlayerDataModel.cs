using Cofoundry.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccedeTeams.Domain
{
    public class PlayerDataModel : ICustomEntityDataModel
    {
        [Display(Description = "short note about player")]
        public string Description { get; set; }

        [Display(Name = "Features", Description = "stand out features of this player")]
        [CustomEntityCollection(FeatureCustomEntityDefinition.DefinitionCode)]
        public ICollection<int> FeatureIds { get; set; }

        [Display(Name = "Images", Description = "profile picture")]
        [ImageCollection]
        public ICollection<int> ImageAssetIds { get; set; }
    }
}
