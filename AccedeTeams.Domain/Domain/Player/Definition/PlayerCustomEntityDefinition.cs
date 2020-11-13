using Cofoundry.Domain;
using System.Collections.Generic;

namespace AccedeTeams.Domain
{
    public class PlayerCustomEntityDefinition : ICustomEntityDefinition<PlayerDataModel>, ICustomizedTermCustomEntityDefinition
    {
        public const string DefinitionCode = "PLAYER";

        public string CustomEntityDefinitionCode => DefinitionCode;

        public string Name => "Player";

        public string NamePlural => "Players";

        public string Description => "Each player can be rated by the public.";

        public bool ForceUrlSlugUniqueness => false;

        public bool AutoGenerateUrlSlug => true;

        public bool AutoPublish => false;

        public bool HasLocale => false;

        public Dictionary<string, string> CustomTerms => new Dictionary<string, string>()
        {
            { CustomizableCustomEntityTermKeys.Title, "Name" }
        };
    }
}
