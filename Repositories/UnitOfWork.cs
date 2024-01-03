using Imobiliare.Entities;
using Imobiliare.Repositories.CommitStrategies;
using Imobiliare.Repositories.Interfaces;
using Logging;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Imobiliare.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UnitOfWork));

        private readonly ApplicationDbContext dbContext;

        /// <summary>
        /// The pre commit strategies
        /// </summary>
        private readonly List<IPreCommitStrategy> preCommitStrategies = new List<IPreCommitStrategy>();

        /// <summary>
        /// The post commit strategies
        /// </summary>
        private readonly List<IPostCommitStrategy> postCommitStrategies = new List<IPostCommitStrategy>();

        public IAnunturiRepository AnunturiRepository { get; }
        public IUsersRepository UsersRepository { get; }
        public IStiriRepository StiriRepository { get; }
        public IJudetRepository JudetRepository { get; }
        public IOrasRepository OrasRepository { get; }
        public ICartierRepository CartierRepository { get; }
        public ILogsRepository LogsRepository { get; }
        public IEmailTemplateRepository EmailTemplateRepository { get; }
        public IConstructoriRepository ConstructoriRepository { get; }
        public IAgentiiRepository AgentiiRepository { get; }
        public ISystemSettingsRepository SystemSettingsRepository { get; }
        public IAuditTrailRepository AuditTrailRepository { get; }
        public IBlockedIpRepository BlockedIpRepository { get; }
        public IMesajRepository MesajRepository { get; }
        public IAnsambluriRepository AnsambluriRepository { get; }
        public IRaportActivitateRepository RaportActivitateRepository { get; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            AnunturiRepository = new AnunturiRepository(dbContext);
            UsersRepository = new UsersRepository(dbContext);
            StiriRepository = new StiriRepository(dbContext);
            JudetRepository = new JudetRepository(dbContext);
            OrasRepository = new OrasRepository(dbContext, JudetRepository);
            CartierRepository = new CartierRepository(dbContext);
            LogsRepository = new LogsRepository(dbContext);
            EmailTemplateRepository = new EmailTemplateRepository(dbContext);
            ConstructoriRepository = new ConstructoriRepository(dbContext);
            AgentiiRepository = new AgentiiRepository(dbContext);
            SystemSettingsRepository = new SystemSettingsRepository(dbContext);
            AuditTrailRepository = new AuditTrailRepository(dbContext);
            BlockedIpRepository = new BlockedIpRepository(dbContext);
            MesajRepository = new MesajRepository(dbContext);
            AnsambluriRepository = new AnsambluriRepository(dbContext);
            RaportActivitateRepository = new RaportActivitateRepository(dbContext);

            this.postCommitStrategies.Add(new AnuntNumberModificatStrategy());
            this.postCommitStrategies.Add(new StiriNumberModificatStrategy());
            this.postCommitStrategies.Add(new LocatiiModificatStrategy());
        }

        public void Complete()
        {
            //var entityChanges = this.DetectChanges();

            //this.preCommitStrategies.ForEach(strategy => strategy.Execute(entityChanges.OfType<EntityChange>().ToList()));

            dbContext.SaveChanges();

            //try
            //{
            //    this.postCommitStrategies.ForEach(strategy => strategy.Execute(entityChanges.OfType<EntityChange>().ToList()));
            //}
            //catch (Exception e)
            //{
            //    log.Error($"PostCommitStrategies error: {e.Message} | InnerException: {e.InnerException?.Message}");
            //}
        }

        public IReadOnlyList<EntityChange> DetectChanges()
        {
            var changes = new List<EntityChange>();
            this.dbContext.ChangeTracker.DetectChanges();

            var objectContext = ((IObjectContextAdapter)this.dbContext).ObjectContext;

            var objectStateEntryList =
              objectContext.ObjectStateManager.GetObjectStateEntries(
                EntityState.Added | EntityState.Modified
                | EntityState.Deleted).ToList();

            foreach (var entry in objectStateEntryList)
            {
                if (!entry.IsRelationship && entry.Entity != null)
                {
                    //Exception here is Userprofile which is no entity
                    if (entry.Entity is Entity)
                    {
                        var entity = (Entity)entry.Entity;
                        var changedProperties = new Dictionary<string, PropertyChange>();
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                changes.Add(EntityEventFactory.Create(entity, EntityState.Added, this, changedProperties));
                                break;

                            case EntityState.Deleted:
                                changes.Add(EntityEventFactory.Create(entity, EntityState.Deleted, this, changedProperties));
                                break;

                            case EntityState.Modified:
                                var modifiedProperties = entry.GetModifiedProperties().Distinct().ToList();

                                modifiedProperties.ForEach(
                                  modifiedPropertyName =>
                                    changedProperties[modifiedPropertyName] =
                                      new PropertyChange(
                                        entry.OriginalValues[modifiedPropertyName],
                                        entry.CurrentValues[modifiedPropertyName]));

                                changes.Add(
                                  EntityEventFactory.Create((Entity)entry.Entity, EntityState.Modified, this, changedProperties));
                                break;
                        }
                    }
                }
                //else
                //{
                //    switch (entry.State)
                //    {
                //        case EntityState.Added:
                //            changes.Add(
                //              EntityEventFactory.Create(
                //                (Entity)objectContext.GetObjectByKey((EntityKey)entry.CurrentValues[0]),
                //                (Entity)objectContext.GetObjectByKey((EntityKey)entry.CurrentValues[1]),
                //                EntityState.Added,
                //                this,
                //                entry.EntitySet.Name));
                //            break;

                //        case EntityState.Deleted:
                //            changes.Add(
                //              EntityEventFactory.Create(
                //                (Entity)objectContext.GetObjectByKey((EntityKey)entry.OriginalValues[0]),
                //                (Entity)objectContext.GetObjectByKey((EntityKey)entry.OriginalValues[1]),
                //                EntityState.Deleted,
                //                this,
                //                entry.EntitySet.Name));
                //            break;

                //        case EntityState.Detached:
                //            throw new InvalidOperationException("Entity in detatched state not supported.");

                //        case EntityState.Modified:
                //            throw new InvalidOperationException("Relationship entry in modified state not expected.");

                //        case EntityState.Unchanged:
                //            // NOP
                //            break;

                //        default:
                //            throw new ArgumentOutOfRangeException();
                //    }

                //}
            }
            return changes;
        }
    }
}