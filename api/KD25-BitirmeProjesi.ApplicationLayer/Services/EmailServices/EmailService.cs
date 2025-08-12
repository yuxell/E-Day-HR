using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.EmailServices
{
    public class EmailService: IEmailService
    {
        //private readonly IConfiguration _configuration;

        ///// <summary>
        ///// EmailService constructor'ı. Uygulama ayarlarından e-posta yapılandırmalarını alır.
        ///// </summary>
        ///// <param name="configuration">appsettings.json içeriğini okumak için kullanılan yapılandırma nesnesi</param>
        //public EmailService(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        ///// <summary>
        ///// Belirtilen e-posta adresine HTML formatında e-posta gönderir.
        ///// </summary>
        ///// <param name="to">Alıcı e-posta adresi</param>
        ///// <param name="subject">E-posta başlığı (konusu)</param>
        ///// <param name="htmlMessage">HTML formatında e-posta içeriği</param>
        ///// <returns>Asenkron Task</returns>
        //public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        //{
        //    // "EmailSettings" başlığındaki ayarları appsettings.json dosyasından okur
        //    var emailSettings = _configuration.GetSection("EmailSettings");

        //    // SMTP istemcisi yapılandırılır
        //    var smtpClient = new SmtpClient
        //    {
        //        Host = emailSettings["Host"],                               // SMTP sunucu adresi (örneğin smtp.gmail.com)
        //        Port = int.Parse(emailSettings["Port"]),                    // SMTP portu (örneğin 587)
        //        EnableSsl = bool.Parse(emailSettings["EnableSsl"]),         // SSL kullanımı (true / false)
        //        Credentials = new NetworkCredential(
        //            emailSettings["Username"],                              // SMTP kullanıcı adı
        //            emailSettings["Password"]                               // SMTP şifresi
        //        )
        //    };

        //    // E-posta mesajı oluşturulur
        //    var mailMessage = new MailMessage
        //    {
        //        From = new MailAddress(emailSettings["From"]),              // Gönderen adres
        //        Subject = subject,                                          // Konu
        //        Body = htmlMessage,                                         // HTML gövde
        //        IsBodyHtml = true                                           // HTML içeriğin etkinleştirilmesi
        //    };

        //    // Alıcı adres eklenir
        //    mailMessage.To.Add(to);

        //    // E-posta asenkron şekilde gönderilir
        //    await smtpClient.SendMailAsync(mailMessage);
        //}


        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Email ayarlarını appsettings.json'dan oku
            string smtpServer = _configuration["EmailSettings:SmtpServer"];
            int smtpPort = int.TryParse(_configuration["EmailSettings:SmtpPort"], out int port) ? port : 587;
            string smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            string senderEmail = _configuration["EmailSettings:SenderEmail"];
            string senderPassword = _configuration["EmailSettings:SenderPassword"];

            using var client = new SmtpClient
            {
                Host = smtpServer,
                Port = smtpPort,
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUsername, senderPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }

    }
}
