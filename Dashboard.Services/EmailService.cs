using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Security;

namespace Dashboard.Services
{
    public class EmailService
    {
        private IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string Subject, string body)
        {
            string from = _configuration["EmailSettings:User"];
            string password = _configuration["EmailSettings:Password"];
            string SMTP = _configuration["EmailSettings:SMTP"];
            int PORT = Int32.Parse(_configuration["EmailSettings:PORT"]);
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = Subject;
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            email.Body = bodyBuilder.ToMessageBody();
            using(var smtpClient = new SmtpClient()) 
            {
                smtpClient.Connect(SMTP, PORT, SecureSocketOptions.SslOnConnect);
                smtpClient.Authenticate(from,password);
               await smtpClient.SendAsync(email);
                smtpClient.Disconnect(true);
            }
        }
    }
}
