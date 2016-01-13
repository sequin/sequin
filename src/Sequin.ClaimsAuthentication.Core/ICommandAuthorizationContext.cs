namespace Sequin.ClaimsAuthentication.Core
{
    public interface ICommandAuthorizationContext
    {
        void Reject();
        bool HasClaim(string type, string value);
    }
}