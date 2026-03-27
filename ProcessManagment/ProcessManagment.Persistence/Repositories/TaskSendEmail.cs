using Microsoft.Extensions.Options;
using ProcessManagment.Application.DTO;
using ProcessManagment.Application.interfaces;
using ProcessManagment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagment.Persistence.Repositories
{
    public class TaskSendEmail: ITaskSendEmail
    {
        private readonly EmailSettings _emailSettings;

        public TaskSendEmail(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmail(EmailModel email)
        {
            using var smtp = new System.Net.Mail.SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                Credentials = new System.Net.NetworkCredential(
                    _emailSettings.SenderEmail,
                    _emailSettings.SenderPassword),
                EnableSsl = true
            };

            var mail = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress(_emailSettings.SenderEmail),
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = true
            };

            mail.To.Add(email.To);

            await smtp.SendMailAsync(mail);
        }


    }
}
