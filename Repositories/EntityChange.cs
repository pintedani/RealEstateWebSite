using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Imobiliare.Entities;

namespace Imobiliare.Repositories
{
    /// <summary>
    /// Represents a non typed entity change.
    /// </summary>
    public abstract class EntityChange
    {
        /// <summary>
        /// Gets or sets the changed properties.
        /// </summary>
        public IReadOnlyDictionary<string, PropertyChange> ChangedProperties { get; protected set; }

        /// <summary>
        /// Gets the untyped Entity.
        /// </summary>
        public Entity Object { get; internal set; }
    }
}