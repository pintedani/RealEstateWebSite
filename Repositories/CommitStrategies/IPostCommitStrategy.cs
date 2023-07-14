using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imobiliare.Repositories.CommitStrategies
{
    /// <summary>
    /// Strategy to execute after successfully saving data in the database.
    /// </summary>
    public interface IPostCommitStrategy
    {
        /// <summary>
        /// Executes the strategy with <see cref="EntityChange"/>.
        /// </summary>
        /// <param name="entityChanges">The entity changes.</param>
        void Execute(IReadOnlyList<EntityChange> entityChanges);
    }
}
