using System.Net;
using System.Net.Mail;

namespace Services.Functions
{
    public interface ISendRecoverPasswordEmailService
    {
        Task SendEmail(string recipientEmail, string token);
    }

    public class SendRecoverPasswordEmailService(string senderEmail, string url, string senderPassword, string host) : ISendRecoverPasswordEmailService
    {
        public async Task SendEmail(string recipientEmail, string token)
        {
            try
            {
                MailMessage mail = new(senderEmail, recipientEmail)
                {
                    Subject = "GamesCatalog - Password Recovery",
                    Body = $"<h2><a href='{url}/User/RecoverPassword/{token}'>Follow this link to change your Password</a></h2>",
                    IsBodyHtml = true
                };

                SmtpClient smtpClient = new(host)
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                };

                try
                {
                    // Send the email
                    await smtpClient.SendMailAsync(mail);
                    Console.WriteLine("Email sent successfully!");
                }
                catch
                {
                    throw;
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
