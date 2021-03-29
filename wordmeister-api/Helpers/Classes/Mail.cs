using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using wordmeister_api.Dtos;
using wordmeister_api.Interfaces;

namespace wordmeister_api.Helpers.Classes
{
    public class Mail : INotification
    {
        private readonly Appsettings _appSettings;
        private SmtpClient _client;

        public Mail(IOptions<Appsettings> appSettings)
        {
            _appSettings = appSettings.Value;

            _client = new SmtpClient
            {
                Host = _appSettings.Mail.Host,
                Port = _appSettings.Mail.Port,
                EnableSsl = _appSettings.Mail.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_appSettings.Mail.EmailPassword, _appSettings.Mail.EmailPassword)
            };

        }

        public void Message(string body, string subject, string to)
        {
            Task.Run(() => { SendMail(body, subject, to); });
        }

        private void SendMail(string body, string subject, string to)
        {
            using (var message = new MailMessage(_appSettings.Mail.EmailPassword, to)
            {
                Subject = subject,
                Body = body
            })
            {
                _client.Send(message);
            }
        }
    }
}
