using lib.Interfaces;
using lib.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailService.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MailService.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;
        private readonly ILogger<MailService> _logger;

        public MailService(IOptions<MailSettings> options, ILogger<MailService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            _logger.LogInformation($"Sending mail to {mailRequest.ToEmail}");
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_settings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] filebytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var memorystream = new MemoryStream())
                        {
                            file.CopyTo(memorystream);
                            filebytes = memorystream.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, filebytes, ContentType.Parse(file.ContentType));
                    }

                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            if (_settings.DisplayName == "freesmtpservers")
            {
                smtp.Connect(_settings.Host, _settings.Port, SecureSocketOptions.None);
            }
            else
            {
                smtp.Connect(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_settings.Mail, _settings.Password);
            }
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            _logger.LogInformation($"Done sending mail to {mailRequest.ToEmail}");
        }
    }
}
