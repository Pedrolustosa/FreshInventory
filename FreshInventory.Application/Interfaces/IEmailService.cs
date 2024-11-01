namespace FreshInventory.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string subject, string body);
}
