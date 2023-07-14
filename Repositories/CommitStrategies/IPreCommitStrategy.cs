using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imobiliare.Repositories.CommitStrategies
{
    /// <summary>
    /// Strategy to execute before saving data in the database.
    /// </summary>
    public interface IPreCommitStrategy
    {
        /// <summary>
        /// Executes the strategy with <see cref="EntityChange"/>.
        /// </summary>
        /// <param name="entityChanges">The entity changes.</param>
        void Execute(IReadOnlyList<EntityChange> entityChanges);
    }
}
