namespace Sequin.ClaimsAuthentication.Core
{
    using System;

    public class AmbiguousCommandAuthorizationException : Exception
    {
        public AmbiguousCommandAuthorizationException(Type commandType) : base($"Command '{commandType.Name}' has ambiguous authorization configuration")
        {
            CommandType = commandType;
        }

        public Type CommandType { get; }
    }
}