using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Application.Common.AccountConfirmation;

public class EmailAsyncMessageSender : IAsyncMessageSender
{
    private EmailMessageSenderOptions _options { get; set; }

    public EmailAsyncMessageSender(IOptions<EmailMessageSenderOptions> options)
    {
        _options = options.Value;
    }
    
    public async Task SendAsync(string email, string subject, string message)
    {
        using var emailMessage = new MimeMessage();
 
        emailMessage.From.Add(new MailboxAddress("MyAtelier", _options.Email));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_options.Email, _options.Password);
        client.AuthenticationMechanisms.Remove("XOAUTH2");
        await client.SendAsync(emailMessage);
 
        await client.DisconnectAsync(true);
    }
}