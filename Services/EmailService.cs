using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

public class EmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress("Abson Energy", "ravjanihasanain@gmail.com"));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        email.Body = new TextPart("html")
        {
            Text = body
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync("smtp.gmail.com", 587, false);

        // Gmail App Password (NOT normal password)
        await smtp.AuthenticateAsync("ravjanihasanain@gmail.com", "temylhbcjhfgdkgs");

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
