namespace Imobiliare.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Imobiliare.Entities;
  using Imobiliare.Repositories.Interfaces;

  public class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(EmailTemplateRepository));

    public EmailTemplateRepository(ApplicationDbContext dbContext) : base(dbContext, new SortSpec(nameof(EmailTemplate.Id)))
    {
    }

    public IEnumerable<EmailTemplate> GetAllEmailTemplates()
    {
      return this.DbContext.EmailTemplates.ToList();
    }

    public EmailTemplate GetEmailTemplate(int id)
    {
      return this.DbContext.EmailTemplates.FirstOrDefault(x => x.Id == id);
    }

    public EmailTemplate GetEmailTemplate(string humanReadableId)
    {
      return this.DbContext.EmailTemplates.FirstOrDefault(x => x.HumanReadableEmailIdentifier == humanReadableId);
    }

    public void AddEmailTemplate(EmailTemplate emailTemplate)
    {
      if (this.DbContext.EmailTemplates.Any(x => x.HumanReadableEmailIdentifier == emailTemplate.HumanReadableEmailIdentifier))
      {
        throw new ArgumentException("Attempt to add Duplicate human readable email identifier");
      }
      this.DbContext.EmailTemplates.Add(emailTemplate);
    }

    public void UpdateEmailTemplate(EmailTemplate emailTemplate)
    {

      var entity = this.DbContext.EmailTemplates.First(x => x.Id == emailTemplate.Id);
      entity.Titlu = emailTemplate.Titlu;
      entity.Mesaj = emailTemplate.Mesaj;
      entity.DateCreated = emailTemplate.DateCreated;
      entity.EmailTemplateCategory = emailTemplate.EmailTemplateCategory;
      entity.HumanReadableEmailIdentifier = emailTemplate.HumanReadableEmailIdentifier;
      entity.DateModified = DateTime.Now;

      //Inainte era doar asta dar dadea eroare "The conversion of a datetime2 data type to a datetime data type resulted in an out-of-range value"
      //db.Entry(emailTemplate).State = EntityState.Modified;
    }

    public void DeleteEmailTemplate(int id)
    {
      var emailTemplate = this.DbContext.EmailTemplates.Find(id);
      this.DbContext.EmailTemplates.Remove(emailTemplate);
    }
  }
}
