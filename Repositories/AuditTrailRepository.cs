namespace Imobiliare.Repositories
{
    using System;

    using Imobiliare.Entities;
    using Imobiliare.Repositories.Interfaces;
    using Logging;
    using Microsoft.EntityFrameworkCore;

    public class AuditTrailRepository : Repository<AuditTrail>, IAuditTrailRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AuditTrailRepository));

        public AuditTrailRepository(ApplicationDbContext dbContext)
          : base(dbContext, new SortSpec(nameof(AuditTrail.Id)))
        {
        }

        public void AddAuditTrail(AuditTrailCategory auditTrailCategory, string message, string userName = null, bool notifyAdmin = false)
        {
            this.DbContext.AuditTrail.Add(
              new AuditTrail()
              {
                  Category = auditTrailCategory,
                  Date = DateTime.Now,
                  Message = message,
                  UserName = userName,
                  NotifyAdmin = notifyAdmin
              });
            log.Info($"AuditTrail: {auditTrailCategory} - {message} - {userName} - notifyAdmin {notifyAdmin}");
        }

        public int DeleteAuditTrailsOltherThanDate(DateTime parse)
        {
            string sqlFormattedDate = parse.ToString("yyyy-MM-dd HH:mm:ss");
            //Be sure to check Db Name: "Log" or "Logs"

            //For SQLite
            return this.DbContext.Database.ExecuteSqlRaw($"DELETE FROM dbo.AuditTrail WHERE Date < '{sqlFormattedDate}'");

            //For SQLServer
            //return this.DbContext.Database.ExecuteSql($"DELETE FROM dbo.AuditTrail WHERE Date < '{sqlFormattedDate}'");

        }
    }
}
