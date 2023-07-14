namespace Imobiliare.Repositories.Interfaces
{
  using System.Collections.Generic;

  using Entities;

  public interface IEmailTemplateRepository : IRepository<EmailTemplate>
  {
    IEnumerable<EmailTemplate> GetAllEmailTemplates();

    EmailTemplate GetEmailTemplate(int id);

    EmailTemplate GetEmailTemplate(string humanReadableId);

    void AddEmailTemplate(EmailTemplate emailTemplate);

    void UpdateEmailTemplate(EmailTemplate emailTemplate);

    void DeleteEmailTemplate(int id);
  }
}
