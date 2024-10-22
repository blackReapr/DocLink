namespace DocLink.Application.Interfaces;

public interface IEmailService
{
    void SendEmail(string email, string subject, string body);
}