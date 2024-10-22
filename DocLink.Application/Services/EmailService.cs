using System.Net;
using System.Net.Mail;
using DocLink.Application.Interfaces;

namespace DocLink.Application.Services;

public class EmailService : IEmailService
{
    public void SendEmail(string email, string subject, string body)
    {
        MailMessage mailMessage = new()
        {
            From = new MailAddress("mehemmed05.aliyev@gmail.com", "AllUp"),
            Subject = subject,
            IsBodyHtml = true
        };
        mailMessage.To.Add(email);


        mailMessage.Body = body;

        SmtpClient smtpClient = new()
        {
            Port = 587,
            Host = "smtp.gmail.com",
            EnableSsl = true,
            Credentials = new NetworkCredential("mehemmed05.aliyev@gmail.com", "pmkt bkfz ntog qxrh")
        };

        smtpClient.Send(mailMessage);
    }
}
