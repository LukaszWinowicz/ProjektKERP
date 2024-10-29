using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;

namespace Services
{
    public class SmtpService
    {
        private readonly SmtpConfig _config;

        public SmtpService(SmtpConfig config)
        {
            _config = config;
        }

        public int SendMail(List<EmailMessage> mailBatch)
        {
            if (mailBatch == null) throw new ArgumentNullException(nameof(mailBatch));

            using (SmtpClient smtpClient = new SmtpClient(_config.SmtpServer, _config.Port))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(_config.Email, _config.Password);

                foreach (var mail in mailBatch)
                {
                    if (mail.SendTo == null || mail.Files == null)
                        throw new ArgumentException("SendTo and Files cannot be null");

                    MailMessage msg = new MailMessage
                    {
                        From = new MailAddress(_config.Email, "Automatic Email"),
                        Subject = mail.Subject,
                        Body = mail.MailBody,
                        IsBodyHtml = true
                    };

                    foreach (var recipient in mail.SendTo)
                    {
                        msg.To.Add(recipient);
                    }

                    foreach (var filePath in mail.Files)
                    {
                        if (File.Exists(filePath))
                        {
                            Attachment attachment = new Attachment(filePath);
                            attachment.Name = Path.GetFileName(filePath);
                            msg.Attachments.Add(attachment);
                        }
                    }

                    smtpClient.Send(msg);
                    Thread.Sleep(5000); // Opcjonalnie, aby uniknąć problemów z limitem wysyłania emaili
                }
            }

            return 0;
        }

        public static bool IsEmailValid(string email)
        {
            string emailRegex = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b";
            return Regex.IsMatch(email, emailRegex);
        }
    }
}