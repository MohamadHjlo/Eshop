using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Daxone_Testing.Data.Repositories
{
    public interface IMessageSender
    {
        public Task SendEmailAsync(string email, string subject, string message, bool isMessageHtml = false);
    }

    public class MessageSender : IMessageSender
    {
        public Task SendEmailAsync(string email, string subject, string message, bool isMessageHtml = false)
        {
            using (var client = new SmtpClient())
            {

                var credentials = new NetworkCredential()
                {
                    UserName = "hjiloomohamad", // without @gmail.com
                    Password = "44353030"
                };

                client.Credentials = credentials;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                using var emailMessage = new MailMessage()
                {
                    To = { new MailAddress(email) },
                    From = new MailAddress("hjiloomohamad@gmail.com"), // with @gmail.com
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = isMessageHtml
                };

                client.Send(emailMessage);
                
            }

            return Task.CompletedTask;
        }
    }
}

