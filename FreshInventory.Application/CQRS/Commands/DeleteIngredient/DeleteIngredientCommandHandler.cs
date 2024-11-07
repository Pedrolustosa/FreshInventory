using MediatR;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;
using FreshInventory.Application.Interfaces;

namespace FreshInventory.Application.CQRS.Commands.DeleteIngredient;

public class DeleteIngredientCommandHandler(IIngredientRepository repository, IEmailService emailService) : IRequestHandler<DeleteIngredientCommand>
{
    private readonly IEmailService _emailService = emailService;
    private readonly IIngredientRepository _repository = repository;

    public async Task Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
    {
        var ingredient = await _repository.GetByIdAsync(request.Id)
            ?? throw new ServiceException($"Ingredient with ID {request.Id} not found.");
        await _repository.DeleteAsync(request.Id);
        var subject = $"Ingredient Removed: {ingredient.Name}";
        var body = $@"
                    <div style='font-family: Arial, sans-serif; color: #333;'>
                        <h2 style='color: #F44336;'>Ingredient Removed</h2>
                        <p>The following ingredient has been removed:</p>
                        <table style='width: 100%; border-collapse: collapse;'>
                            <tr>
                                <td style='font-weight: bold; padding: 8px; border: 1px solid #ddd;'>Name:</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{ingredient.Name}</td>
                            </tr>
                            <tr>
                                <td style='font-weight: bold; padding: 8px; border: 1px solid #ddd;'>Quantity:</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{ingredient.Quantity}</td>
                            </tr>
                            <tr>
                                <td style='font-weight: bold; padding: 8px; border: 1px solid #ddd;'>Unit:</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{ingredient.Unit}</td>
                            </tr>
                            <tr>
                                <td style='font-weight: bold; padding: 8px; border: 1px solid #ddd;'>Category:</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{ingredient.Category}</td>
                            </tr>
                            <tr>
                                <td style='font-weight: bold; padding: 8px; border: 1px solid #ddd;'>Expiry Date:</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>{ingredient.ExpiryDate.ToShortDateString()}</td>
                            </tr>
                        </table>
                        <p style='margin-top: 20px; color: #F44336;'>This ingredient has been permanently removed from the system.</p>
                    </div>";
        await _emailService.SendEmailAsync(subject, body);
    }
}
