namespace Sample.Web.ClaimsAuthentication.Commands
{
    using Sequin.ClaimsAuthentication.Core;

    [AnonymousCommand]
    public class MyAnonymousCommand
    {
        public int PropertyA { get; set; }
        public int PropertyB { get; set; }
    }
}