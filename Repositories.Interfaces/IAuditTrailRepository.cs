using System;

namespace Imobiliare.Repositories.Interfaces
{
  using Imobiliare.Entities;
  public interface IAuditTrailRepository : IRepository<AuditTrail>
  {
    /// <summary>
    /// Adds an audit trail about site events
    /// </summary>
    /// <param name="auditTrailCategory"></param>
    /// <param name="message"></param>
    /// <param name="userName"></param>
    /// <param name="notifyAdmin">If true, this is included in the reports email which is sent periodically to admins</param>
    void AddAuditTrail(AuditTrailCategory auditTrailCategory, string message, string userName = null, bool notifyAdmin = false);

    int DeleteAuditTrailsOltherThanDate(DateTime parse);
  }
}
