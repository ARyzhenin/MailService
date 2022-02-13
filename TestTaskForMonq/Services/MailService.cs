using System;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Mails.DtoControllerModels;
using Mails.Models;
using Mails.Repository;
using MimeKit;

namespace Mails.Services
{
    public interface IMailService
    {
        public Task SendMailAsync(MailInfoDto mailInfo);
    }


    public class MailService : IMailService

    {
        private readonly ILogRepository _repository;

        public MailService(ILogRepository repository)
        {
            _repository = repository;
        }

        public async Task SendMailAsync(MailInfoDto model)
        {
            var body = model.Body;

            var subject = model.Subject;

            var recipients = model.Recipients;

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

            foreach (var recipient in recipients)
            {
                try
                {
                    var log = new Log
                    {
                        Body = model.Body,
                        Recipient = recipient,
                        Subject = model.Subject,
                        DateOfCreation = DateTime.Now,
                        FailedMessage = ProcessDeliveryStatusNotification(emailMessage)
                    };

                    if (log.FailedMessage != null)
                    {
                        log.Result = Status.Failed;
                    }
                    else
                    {
                        log.Result = Status.OK;
                    }

                    _repository.PostLog(log);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }

        }

        public string ProcessDeliveryStatusNotification(MimeMessage message)
        {
            var report = message.Body as MultipartReport;

            if (report == null || report.ReportType == null || !report.ReportType.Equals("delivery-status", StringComparison.OrdinalIgnoreCase))
            {

                return null;
            }

            // process the report
            foreach (var mds in report.OfType<MessageDeliveryStatus>())
            {
                // process the status groups - each status group represents a different recipient

                // The first status group contains information about the message
                var envelopeId = mds.StatusGroups[0]["Original-Envelope-Id"];

                // all of the other status groups contain per-recipient information
                for (int i = 1; i < mds.StatusGroups.Count; i++)
                {
                    var recipient = mds.StatusGroups[i]["Original-Recipient"];
                    var action = mds.StatusGroups[i]["Action"];

                    if (recipient == null)
                        recipient = mds.StatusGroups[i]["Final-Recipient"];

                    // the recipient string should be in the form: "rfc822;user@domain.com"
                    var index = recipient.IndexOf(';');
                    var address = recipient.Substring(index + 1);

                    switch (action)
                    {
                        case "failed":
                            return $"Delivery of message {envelopeId} failed for { address}";

                        case "delayed":
                            return $"Delivery of message {envelopeId} delayed for { address}";

                        case "delivered":
                            return $"Delivery of message {envelopeId} delivered for { address}";
                        case "relayed":
                            return $"Delivery of message {envelopeId} relayed for { address}";
                        case "expanded":
                            return $"Delivery of message {envelopeId} expanded for { address}";
                    }
                }
            }
            return null;
        }
    }
}