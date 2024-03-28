using System.Net.Mail;
using System.Net;

namespace BeautySaloon.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string? _mailAppLogin;

        public EmailService()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
            string? smtpMailServer = configuration.GetSection("MailConfig").GetSection("SmtpMailServer").Value;
            _mailAppLogin = configuration.GetSection("MailConfig").GetSection("MailAppLogin").Value;
            string? mailAppPassword = configuration.GetSection("MailConfig").GetSection("MailAppPassword").Value;
            bool enableSSL = configuration.GetSection("MailConfig").GetValue<bool>("EnableSSL");
            int smtpPort = configuration.GetSection("MailConfig").GetValue<int>("smtpPort");


            _smtpClient = new SmtpClient(smtpMailServer);
            _smtpClient.Credentials = new NetworkCredential(_mailAppLogin, mailAppPassword);
            _smtpClient.EnableSsl = enableSSL;
            _smtpClient.Port = smtpPort;
        }

        public void SendMail(string targetEmail, string topic, string content)
        {
         
            _smtpClient.Send(_mailAppLogin, targetEmail, topic, content);

        }
    }
}
