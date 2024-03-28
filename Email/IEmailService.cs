namespace BeautySaloon.Email
{
    public interface IEmailService
    {
        public void SendMail(string targetEmail, string topic, string content);

    }
}
