namespace FreshInventory.Application.DTO;

public record EmailSettingsDto(string From, string SmtpServer, string UserName, string Password);
