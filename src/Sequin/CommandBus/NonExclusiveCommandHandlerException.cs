namespace Sequin.CommandBus
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Core.Infrastructure;

    public class NonExclusiveCommandHandlerException : CommandHandlerException
    {
        private readonly IEnumerable<Type> _resolvedServiceTypes;

        internal NonExclusiveCommandHandlerException(Type command, IEnumerable<Type> resolvedServiceTypes) : base(command, "Multiple handlers exist for the given command type.")
        {
            _resolvedServiceTypes = resolvedServiceTypes;
        }

        public IReadOnlyCollection<Type> ResolvedServiceTypes => new ReadOnlyCollection<Type>(_resolvedServiceTypes.ToList());
    }
}