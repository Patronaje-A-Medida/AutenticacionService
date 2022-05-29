using AutenticacionService.Business.Handlers;
using AutenticacionService.Business.Utils;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static AutenticacionService.Domain.Utils.ErrorsUtil;

namespace AutenticacionService.Business.ServiceSmtp
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(EmailConfiguration.UserName, EmailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Content
            };

            return emailMessage;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            using(var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(EmailConfiguration.SmtpServerMail, EmailConfiguration.PortMail, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(EmailConfiguration.UserName, EmailConfiguration.Password);
                    await client.SendAsync(mailMessage);
                }
                catch(Exception ex)
                {
                    throw new ServiceException(
                        HttpStatusCode.InternalServerError, 
                        ErrorsCode.ERROR_SEND_EMAIL, 
                        ErrorMessages.ERROR_SEND_EMAIL);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }

        }
    }
}
