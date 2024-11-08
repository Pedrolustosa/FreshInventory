using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using FreshInventory.Application.DTO;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettingsDto _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(EmailSettingsDto emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings;
            _logger = logger;
        }

        public async Task SendEmailAsync(string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.From, _emailSettings.From));
            message.To.Add(new MailboxAddress("", "pedroeternalss@gmail.com"));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();

            try
            {
                _logger.LogInformation("Connecting to SMTP server {SmtpServer}:{Port}.", _emailSettings.SmtpServer, _emailSettings.SmtpServer);
                await client.ConnectAsync(_emailSettings.SmtpServer, 587, SecureSocketOptions.StartTls);

                _logger.LogInformation("Authenticating with SMTP server.");
                await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);

                _logger.LogInformation("Sending email to {Recipient}.", message.To.ToList());
                await client.SendAsync(message);

                _logger.LogInformation("Email '{Subject}' sent successfully to {Recipient}.", subject, message.To.ToList());
            }
            catch (SmtpCommandException ex)
            {
                _logger.LogError(ex, "SMTP command error while sending email '{Subject}' to {Recipient}.", subject, message.To.ToList());
                throw new EmailException("An SMTP command error occurred while sending the email.", ex);
            }
            catch (SmtpProtocolException ex)
            {
                _logger.LogError(ex, "SMTP protocol error while sending email '{Subject}' to {Recipient}.", subject, message.To.ToList());
                throw new EmailException("An SMTP protocol error occurred while sending the email.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while sending email '{Subject}' to {Recipient}.", subject, message.To.ToList());
                throw new EmailException("An unexpected error occurred while sending the email.", ex);
            }
            finally
            {
                if (client.IsConnected)
                {
                    _logger.LogInformation("Disconnecting from SMTP server.");
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
