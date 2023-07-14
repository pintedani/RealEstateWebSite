namespace Imobiliare.Repositories
{
  using System;

  using Imobiliare.Entities;
  using Imobiliare.Repositories.Interfaces;

  public class AuditTrailRepository : Repository<AuditTrail>, IAuditTrailRepository
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AuditTrailRepository));

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
      log.InfoFormat($"AuditTrail: {auditTrailCategory} - {message} - {userName} - notifyAdmin {notifyAdmin}");
    }

    public int DeleteAuditTrailsOltherThanDate(DateTime parse)
    {
      string sqlFormattedDate = parse.ToString("yyyy-MM-dd HH:mm:ss");
      //Be sure to check Db Name: "Log" or "Logs"
      return this.DbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.AuditTrail WHERE Date < '" + sqlFormattedDate + "'");
    }
  }
}
