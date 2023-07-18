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
    public class LocatiiModificatStrategy : IPostCommitStrategy
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LocatiiModificatStrategy));

        public void Execute(IReadOnlyList<EntityChange> entityChanges)
        {
            bool rebuildCache = false;
            foreach (var entityChange in entityChanges.ToList())
            {
                var entityType = entityChange.Object.GetEntityType();

                if (entityType == typeof(Oras))
                {
                    if (entityChange is EntityAdded<Oras>)
                    {
                        var entityEvent = (EntityAdded<Oras>)entityChange;
                        var entity = entityEvent.Entity;
                        log.Debug($"{nameof(LocatiiModificatStrategy)} a detectat localitate adaugata: {entity.Nume}");
                        rebuildCache = true;
                        break;
                    }

                    if (entityChange is EntityUpdated<Oras>)
                    {
                        var entityEvent = (EntityUpdated<Oras>)entityChange;
                        var entity = entityEvent.Entity;
                        log.Debug($"{nameof(LocatiiModificatStrategy)} a detectat localitate modificata: {entity.Nume}");

                        foreach (var changedProperty in entityEvent.ChangedProperties)
                        {
                            log.Debug($"{nameof(LocatiiModificatStrategy)} a detectat localitate property modificata: {changedProperty.Key} : {changedProperty.Value.CurrentValue}");
                        }
                        rebuildCache = true;
                        break;
                    }

                    if (entityChange is EntityDeleted<Oras>)
                    {
                        log.Debug($"{nameof(LocatiiModificatStrategy)} a detectat localitate stearsa");
                        rebuildCache = true;
                        break;
                    }
                }

                if (entityType == typeof(Judet))
                {
                    if (entityChange is EntityAdded<Judet>)
                    {
                        var entityEvent = (EntityAdded<Judet>)entityChange;
                        var entity = entityEvent.Entity;
                        log.Debug($"{nameof(LocatiiModificatStrategy)} a detectat judet adaugat: {entity.Nume}");
                        rebuildCache = true;
                        break;
                    }

                    if (entityChange is EntityUpdated<Judet>)
                    {
                        var entityEvent = (EntityUpdated<Judet>)entityChange;
                        var entity = entityEvent.Entity;
                        log.Debug($"{nameof(LocatiiModificatStrategy)} a detectat judet modificat: {entity.Nume}");

                        foreach (var changedProperty in entityEvent.ChangedProperties)
                        {
                            log.Debug($"{nameof(LocatiiModificatStrategy)} a detectat judet property modificat {changedProperty.Key} : {changedProperty.Value.CurrentValue}");
                        }
                        rebuildCache = true;
                        break;
                    }

                    if (entityChange is EntityDeleted<Judet>)
                    {
                        log.Debug($"{nameof(LocatiiModificatStrategy)} a detectat judet sters");
                        rebuildCache = true;
                        break;
                    }
                }

                if (entityType == typeof(Cartier))
                {
                    if (entityChange is EntityAdded<Cartier>)
                    {
                        var entityEvent = (EntityAdded<Cartier>)entityChange;
                        var entity = entityEvent.Entity;
                        log.Debug($"{nameof(LocatiiModificatStrategy)} a detectat cartier adaugat: {entity.Nume}");
                        rebuildCache = true;
                        break;
                    }

                    if (entityChange is EntityUpdated<Cartier>)
                    {
                        var entityEvent = (EntityUpdated<Cartier>)entityChange;
                        var entity = entityEvent.Entity;
                        log.Debug($"{nameof(LocatiiModificatStrategy)} a detectat cartier modificat: {entity.Nume}");

                        foreach (var changedProperty in entityEvent.ChangedProperties)
                        {
                            log.Debug($"{nameof(LocatiiModificatStrategy)} a detectat cartier modificat {changedProperty.Key} : {changedProperty.Value.CurrentValue}");
                        }
                        rebuildCache = true;
                        break;
                    }

                    if (entityChange is EntityDeleted<Cartier>)
                    {
                        log.Debug($"{nameof(LocatiiModificatStrategy)} a detectat cartier sters");
                        rebuildCache = true;
                        break;
                    }
                }
            }

            if (rebuildCache)
            {
                log.Debug($"{nameof(LocatiiModificatStrategy)} Se reseteaza cache");
                LocatiiCaching.Reset();
            }
        }
    }
}
