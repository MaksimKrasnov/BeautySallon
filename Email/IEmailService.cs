namespace BeautySaloon.Email
{
    public interface IEmailService
    {
        public Task SendMail(string targetEmail, string topic, string content);

    }
}
