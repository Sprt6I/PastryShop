using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace PastryServer.Services
{
    public class Gmail_Sender
    {
        public async Task Send_Email_Async(string app_password, string from_gmail, string to_gmail, string subject, string html_body)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(from_gmail));
            message.To.Add(MailboxAddress.Parse(to_gmail));
            message.Subject = subject;

            message.Body = new BodyBuilder { HtmlBody = $"{html_body}" }.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(from_gmail, app_password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
