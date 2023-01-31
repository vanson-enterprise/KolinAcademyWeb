using KA.EmailService.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.EmailService.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSenderConfiguration _mailSenderConfig;
        public EmailSender(MailSenderConfiguration emailConfig)
        {
            _mailSenderConfig = emailConfig;
        }

        public void SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);

            await SendAsync(emailMessage);
        }

        #region private
        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("KolinAcademy",_mailSenderConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(_mailSenderConfig.SmtpServer, _mailSenderConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_mailSenderConfig.UserName, _mailSenderConfig.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    // Log ex
                    Directory.CreateDirectory("mailssave");
                    var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                    mailMessage.WriteTo(emailsavefile);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using(var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_mailSenderConfig.SmtpServer, _mailSenderConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_mailSenderConfig.UserName, _mailSenderConfig.Password);
                    await client.SendAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    // Log ex
                    Directory.CreateDirectory("mailssave");
                    var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                    await mailMessage.WriteToAsync(emailsavefile);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
        
        #endregion
    }
}
