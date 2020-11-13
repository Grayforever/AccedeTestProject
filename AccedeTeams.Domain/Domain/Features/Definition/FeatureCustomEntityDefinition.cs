using Cofoundry.Domain;

namespace AccedeTeams.Domain
{
    public class FeatureCustomEntityDefinition : ICustomEntityDefinition<FeatureDataModel>
    {
        public const string DefinitionCode = "ACTEAM";

        public string CustomEntityDefinitionCode => DefinitionCode;

        public string Name => "Feature";

        public string NamePlural => "Features";

        public string Description => "Team players unique features";

        public bool ForceUrlSlugUniqueness => true;

        public bool AutoGenerateUrlSlug => true;


        public bool AutoPublish => true;

        public bool HasLocale => false;
    }
}
