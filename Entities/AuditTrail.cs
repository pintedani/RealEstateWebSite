namespace Imobiliare.Entities
{
  using System;
  using System.ComponentModel.DataAnnotations.Schema;

  public class AuditTrail : Entity
  {
    public AuditTrailCategory Category { get; set; }

    public string Message { get; set; }

    public DateTime Date { get; set; }

    public bool NotifyAdmin { get; set; }

    //[ForeignKey(nameof(UserProfileId))]
    //public UserProfile UserProfile { get; set; }

    //public int? UserProfileId { get; set; }

    public string UserName { get; set; }
  }

  public enum AuditTrailCategory
  {
    Message = 0,
    Warning = 1,
    Error = 2,
    Fatal = 3
  }
}
