using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Mails.DtoControllerModels;
using MimeKit;

namespace Mails.Services
{
    public interface IMailService
    {
        public Task SendMailAsync(MailInfoDto mailInfo);
    }

   
    public class MailService : IMailService
    {
        public async Task SendMailAsync(MailInfoDto mailInfo)
        {
            var body = mailInfo.Body;

            var subject = mailInfo.Subject;

            var recipients = mailInfo.Recipients;

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Имя отправителя", "rav81294@gmail.com"));

            foreach (var recipient in recipients)
            {
                emailMessage.To.Add(new MailboxAddress("", recipient));
            }

            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);

                await client.AuthenticateAsync("rav81294@gmail.com", "hccmajhydjvrahhb");

                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }

        }
    }
}