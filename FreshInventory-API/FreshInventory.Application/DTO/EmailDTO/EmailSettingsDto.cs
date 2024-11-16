namespace FreshInventory.Application.DTO.EmailDTO;

public record EmailSettingsDto(string From, string SmtpServer, string UserName, string Password);
