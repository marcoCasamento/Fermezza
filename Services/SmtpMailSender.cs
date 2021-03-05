using Fermezza.Helpers;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fermezza.Services
{
    public class SmtpMailSender : IEmailSender
    {
        public SmtpMailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } 

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient();
            client.Connect(Options.SmtpServer, Options.SmtpPort, Options.SmtpUseSSL);
            client.Authenticate(Options.SmtpUser, Options.SmtpPassword);


            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("eCasamento Administrator", "admin@ecasamento.com"));
            //msg.To.Add(new MailboxAddress((string)null, email));
            msg.To.Add(new MailboxAddress("Marco Casamento", "marco.casamento@hotmail.it"));
            msg.Subject = subject;
            message += $"email richiedente: {email}";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            
            msg.Body = bodyBuilder.ToMessageBody();
            
            return client.SendAsync(msg);
        }

    }
}
