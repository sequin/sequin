namespace Sequin.ClaimsAuthentication.Core
{
    public interface ICommandAuthorizationContext
    {
        void Reject();
        bool HasClaim(string type, string value);

        bool IsAuthenticated { get; }
        bool IsAuthorized { get; }
    }
}