using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;

namespace Talabat.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting Config;
        public EmailService(IOptions<EmailSetting> options)
        {
            Config = options.Value;
        }
        public async Task SendMailAsync(string To,string Body, string Subject)
        {
            var client = new SmtpClient(Config.SmtpHost)
            {
                Port = Config.SmtpPort,
                EnableSsl = true,
                Credentials = new NetworkCredential(Config.EmailSender, Config.SenderPassword)
            };
            var sendmail = new MailMessage()
            {
                Body = Body,
                From=new MailAddress(Config.EmailSender),
                Subject = Subject
            };
            sendmail.To.Add(To);
            await client.SendMailAsync(sendmail);
        }
    }
}
