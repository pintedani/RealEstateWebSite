using Imobiliare.ServiceLayer.Interfaces;

namespace Imobiliare.ServiceLayer.EmailService
{
  public interface IEmailSender
    {
    EmailSendStatus SendUserEmail(string email, string title, string message, string headerFooter, string userId);
  }
}