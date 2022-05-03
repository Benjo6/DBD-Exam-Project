using DBD_Exam_Project.Settings;
using lib.Interfaces;
using lib.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DBD_Exam_Project.Service
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<MailService> _logger;

        public MailService(IOptions<MailSettings> options,ILogger<MailService> logger)
        {
            _mailSettings = options.Value;
            _logger = logger;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            _logger.LogInformation($"Sending mail to {mailRequest.ToEmail}");
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
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
            if (_mailSettings.DisplayName == "freesmtpservers")
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.None);
            }
            else
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            }
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            _logger.LogInformation($"Done sending mail to {mailRequest.ToEmail}");
        }
    }
}
