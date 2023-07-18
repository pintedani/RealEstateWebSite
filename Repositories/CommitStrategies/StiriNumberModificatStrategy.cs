using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caching;
using Imobiliare.Entities;
using Logging;

namespace Imobiliare.Repositories.CommitStrategies
{
    public class StiriNumberModificatStrategy : IPostCommitStrategy
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StiriNumberModificatStrategy));

        public void Execute(IReadOnlyList<EntityChange> entityChanges)
        {
            bool rebuildCache = false;
            foreach (var entityChange in entityChanges.ToList())
            {
                var entityType = entityChange.Object.GetEntityType();

                if (entityType == typeof(Stire))
                {
                    if (entityChange is EntityAdded<Stire>)
                    {
                        var entityEvent = (EntityAdded<Stire>)entityChange;
                        var stire = entityEvent.Entity;
                        log.Debug($"{nameof(StiriNumberModificatStrategy)} a detectat stire adaugata: {stire.Titlu}");
                        rebuildCache = true;
                        break;
                    }

                    if (entityChange is EntityUpdated<Stire>)
                    {
                        var entityEvent = (EntityUpdated<Stire>)entityChange;
                        var stire = entityEvent.Entity;
                        log.Debug($"{nameof(StiriNumberModificatStrategy)} a detectat stire modificata: {stire.Titlu}");

                        foreach (var changedProperty in entityEvent.ChangedProperties)
                        {
                            log.Debug($"{nameof(StiriNumberModificatStrategy)} a detectat stire property modificat: {changedProperty.Key}");
                        }
                        rebuildCache = true;
                        break;
                    }

                    if (entityChange is EntityDeleted<Stire>)
                    {
                        log.Debug($"{nameof(StiriNumberModificatStrategy)} a detectat stire stearsa");
                        rebuildCache = true;
                        break;
                    }
                }
            }

            if (rebuildCache)
            {
                log.Debug($"{nameof(StiriNumberModificatStrategy)} Se reseteaza cache");
                StiriCaching.Reset();
            }
        }
    }
}
