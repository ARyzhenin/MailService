using System;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using TestTaskForMonq.DtoControllerModels;
using TestTaskForMonq.Models;
using TestTaskForMonq.Repository;
using Microsoft.Extensions.Options;
using MimeKit;

namespace TestTaskForMonq.Services
{
    public interface IMailService
    {
        public Task SendMailAsync(MailInfoDto mailInfo);
    }


    public class MailService : IMailService

    {
        private readonly ILogRepository _repository;
        private readonly IOptions<EmailSettings> _emailSettings;

        public MailService(ILogRepository repository, IOptions<EmailSettings> emailSettings)
        {
            _repository = repository;
            _emailSettings = emailSettings;
        }

        /// <summary>
        /// Sending email
        /// </summary>
        /// <param name="model">Information about message</param>
        /// <returns></returns>
        public async Task SendMailAsync(MailInfoDto model)
        {
            var body = model.Body;

            var subject = model.Subject;

            var recipients = model.Recipients;

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailSettings.Value.SenderName, _emailSettings.Value.SendFrom));


            foreach (var recipient in recipients)
            {
                emailMessage.To.Add(new MailboxAddress("", recipient));
            }

            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = body
            };
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailSettings.Value.Host, _emailSettings.Value.Port, _emailSettings.Value.UseSSL);

                    await client.AuthenticateAsync(_emailSettings.Value.SendFrom, _emailSettings.Value.Password);

                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                var log = new Log
                {
                    Body = model.Body,
                    Recipients = recipients.Select(recipient => new Recipient()
                    {
                        EMailAdress = recipient
                    }).ToList(),
                    Subject = model.Subject,
                    DateOfCreation = DateTime.Now,
                    FailedMessage = ProcessDeliveryStatusNotification(emailMessage)
                };


                if (log.FailedMessage != null)
                {
                    log.Result = Status.Failed.ToString();
                }
                else
                {
                    log.Result = Status.OK.ToString();
                }

                await _repository.PostLogAsync(log);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Notification delivery status method
        /// </summary>
        /// <param name="message">Message we are sending</param>
        /// <returns>Message delivery status</returns>
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