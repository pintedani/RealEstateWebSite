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
    public class AnuntNumberModificatStrategy : IPostCommitStrategy
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AnuntNumberModificatStrategy));

        public void Execute(IReadOnlyList<EntityChange> entityChanges)
        {
            bool rebuildCache = false;

            foreach (var entityChange in entityChanges.ToList())
            {
                var entityType = entityChange.Object.GetEntityType();

                if (entityType == typeof(Imobil))
                {
                    if (entityChange is EntityAdded<Imobil>)
                    {
                        var entityEvent = (EntityAdded<Imobil>)entityChange;
                        var anunt = entityEvent.Entity;
                        log.Debug($"{nameof(AnuntNumberModificatStrategy)} a detectat anunt adaugat: {anunt.Title}");
                        rebuildCache = true;
                        break;
                    }

                    if (entityChange is EntityUpdated<Imobil>)
                    {
                        var entityEvent = (EntityUpdated<Imobil>)entityChange;
                        if (entityEvent.ChangedProperties.All(x => x.Key != nameof(Imobil.NumarVizualizari)))
                        {
                            var anunt = entityEvent.Entity;
                            log.Debug(
                                $"{nameof(AnuntNumberModificatStrategy)} a detectat anunt modificat: {anunt.Title}");

                            foreach (var changedProperty in entityEvent.ChangedProperties)
                            {
                                log.Debug(
                                    $"{nameof(AnuntNumberModificatStrategy)} a detectat modificare: {changedProperty.Key}");

                                if (changedProperty.Key == nameof(Imobil.StareAprobare))
                                {
                                    var currentValue = (StareAprobare) changedProperty.Value.OriginalValue;
                                    var changedValue = (StareAprobare) changedProperty.Value.CurrentValue;
                                    if (currentValue == StareAprobare.Aprobat || changedValue == StareAprobare.Aprobat)
                                    {
                                        log.Debug($"{nameof(AnuntNumberModificatStrategy)} a detectat stare aprobare modificata, MUSAI rebuild cache: {changedProperty.Key} : {changedProperty.Value.OriginalValue} -> {changedProperty.Value.CurrentValue}");
                                        rebuildCache = true;
                                        break;
                                    }
                                }

                                if (changedProperty.Key == nameof(Imobil.Poze))
                                {
                                    var currentValue = changedProperty.Value.OriginalValue;
                                    var changedValue = changedProperty.Value.CurrentValue;
                                    log.Debug($"{nameof(AnuntNumberModificatStrategy)} a detectat poze modificate, MUSAI rebuild cache: {changedProperty.Key} : {changedProperty.Value.OriginalValue} -> {changedProperty.Value.CurrentValue}");
                                    rebuildCache = true;
                                    break;
                                }
                            }
                        }

                        break;
                    }

                    if (entityChange is EntityDeleted<Imobil>)
                    {
                        log.Debug($"{nameof(AnuntNumberModificatStrategy)} a detectat anunt sters");
                        rebuildCache = true;
                        break;
                    }
                }
            }

            if (rebuildCache)
            {
                log.Debug($"{nameof(AnuntNumberModificatStrategy)} Se reseteaza cache");
                AnunturiCaching.Reset();
            }
        }
    }
}
