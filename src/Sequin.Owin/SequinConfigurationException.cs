namespace Sequin.Owin
{
    using System;

    public class SequinConfigurationException : Exception
    {
        public SequinConfigurationException(string property) : base($"Invalid {property} configuration")
        {
            Property = property;
        }

        public string Property { get; }
    }
}