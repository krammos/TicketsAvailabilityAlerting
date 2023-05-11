using System.Text;
using System.Net;
using System.Net.Mail;
using TicketsAvailabilityAlerting.Models;


namespace TicketsAvailabilityAlerting.Services
{
    public interface IMailService
    {
        void SendMail(string[] arrayOfEmails, MailSettings mailSettings);
    }

    /******************************************************************************************************/

    public class MailService : IMailService
    {
        public void SendMail(string[] arrayOfEmails, MailSettings mailSettings)
        {
            using MailMessage mail = new();

            // From
            mail.From = new MailAddress(mailSettings.EmailAddress, "TicketsAvailabilityAlerting");

            // To
            foreach (var email in arrayOfEmails)
            {
                mail.To.Add(email);
            }

            // Subject
            mail.Subject = "----- Email by TicketsAvailabilityAlerting App -----";

            // Body
            StringBuilder template = new();
            template.AppendLine("Tickets are available to the public!");
            mail.IsBodyHtml = false;
            mail.Body = template.ToString();

            // SMTP settings
            using SmtpClient smtp = new(mailSettings.MailServer, mailSettings.MailServerPort);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(mailSettings.EmailAddress, mailSettings.Password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

    } // End of Class
} // End of Namespace