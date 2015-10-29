namespace Sequin.Infrastructure
{
    using System;

    internal class UnidentifiableCommandException : Exception
    {
        public UnidentifiableCommandException() : base("Command type could not be identified from request")
        {
            
        }
    }
}
