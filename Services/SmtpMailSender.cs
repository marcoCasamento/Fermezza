using Fermezza.Helpers;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            var client = new SmtpClient(Options.SmtpServer)
            {
                Port = Options.SmtpPort,
                EnableSsl = Options.SmtpUseSSL,
                Credentials = new NetworkCredential(Options.SmtpUser, Options.SmtpPassword)
            };

            var msg = new MailMessage("admin@ecasamento.com", email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            
            return client.SendMailAsync(msg);
        }

    }
}
