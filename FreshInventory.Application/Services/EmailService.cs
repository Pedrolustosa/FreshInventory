using MimeKit;
using MailKit.Net.Smtp;
using FreshInventory.Application.DTO;
using FreshInventory.Application.Interfaces;

namespace FreshInventory.Application.Services;

public class EmailService(EmailSettingsDto emailSettings) : IEmailService
{
    private readonly EmailSettingsDto _emailSettings = emailSettings;

    public async Task SendEmailAsync(string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.From, _emailSettings.From));
        message.To.Add(new MailboxAddress("", "EMAIL"));
        message.Subject = subject;
        message.Body = new TextPart("html")
        {
            Text = body
        };
        using var client = new SmtpClient();
        await client.ConnectAsync(_emailSettings.SmtpServer, 587, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
