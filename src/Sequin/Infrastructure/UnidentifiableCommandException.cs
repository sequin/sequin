namespace Sequin.Infrastructure
{
    using System;

    public class UnidentifiableCommandException : Exception
    {
        public UnidentifiableCommandException() : base("Command type could not be identified from request")
        {
            
        }
    }
}
