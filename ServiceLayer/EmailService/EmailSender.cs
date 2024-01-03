using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading;
using Imobiliare.ServiceLayer.Interfaces;
using Logging;

namespace Imobiliare.ServiceLayer.EmailService
{
    public class EmailSender : IEmailSender
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmailSender));

        public EmailSendStatus SendUserEmail(string email, string title, string message, string headerFooter, string userId)
        {
            //TODO reenable
            //var fromAddress = new MailAddress(ConfigurationManager.AppSettings["EmailAdminFromAddress"], ConfigurationManager.AppSettings["EmailAdminFromDisplayName"]);
            //var toAddress = new MailAddress(email, email);
            //string fromPassword = ConfigurationManager.AppSettings["EmailAdminFromPassword"];
            //string subject = title;
            //string body = string.Format(headerFooter, message, userId);

            //var smtp = new SmtpClient
            //{
            //    Host = ConfigurationManager.AppSettings["SmtpClientHost"],
            //    Port = int.Parse(ConfigurationManager.AppSettings["SmtpClientPort"]),
            //    //EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            //};

            //Thread emailSendingThread = new Thread(delegate ()
            //{
            //    using (var mailMessage = new MailMessage(fromAddress, toAddress)
            //    {
            //        Subject = subject,
            //        Body = body,
            //        Priority = MailPriority.Normal,
            //        IsBodyHtml = true
            //    })
            //    {
            //        try
            //        {
            //            log.DebugFormat($"Se trimite mesaj catre {email} cu titlul {title} si mesajul {message}");
            //            smtp.Send(mailMessage);
            //        }
            //        catch (SmtpException ex)
            //        {
            //            log.Error($"(SmtpException)Error occured during sending email message to {email} with title {title} error: {ex.Message}");
            //            //throw;
            //            //Return something else?
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error($"General Error occured during sending email message to {email} with title {title} error: {ex.Message}");
            //            //throw;
            //        }
            //    }
            //});

            //emailSendingThread.Start();

            return EmailSendStatus.Success;
        }
    }
}