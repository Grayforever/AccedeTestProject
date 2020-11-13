using Cofoundry.Core.Mail;

namespace AccedeTeams.Domain
{
    public class NewUserWelcomeMailTemplate : IMailTemplate
    {
        public string FirstName { get; set; }

        public string ViewFile
        {
            get { return "~/MailTemplates/NewUserWelcomeMail"; }
        }

        public string Subject
        {
            get { return "Welcome to Accede Teams!"; }
        }
    }
}
