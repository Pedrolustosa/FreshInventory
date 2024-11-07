using MediatR;
using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Interfaces;

namespace FreshInventory.Application.CQRS.Commands.CreateIngredient;

public class CreateIngredientCommandHandler(IIngredientRepository repository, IMapper mapper, IEmailService emailService) : IRequestHandler<CreateIngredientCommand, int>
{
    private readonly IMapper _mapper = mapper;
    private readonly IEmailService _emailService = emailService;
    private readonly IIngredientRepository _repository = repository;

    public async Task<int> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
    {
        var ingredient = _mapper.Map<Ingredient>(request.Ingredient);
        await _repository.AddAsync(ingredient);
        var subject = $"New Ingredient Added: {ingredient.Name}";
        var body = $@"
                        <div style='font-family: Arial, sans-serif; color: #333;'>
                            <h2 style='color: #4CAF50;'>New Ingredient Added</h2>
                            <p>The following ingredient has been successfully added:</p>
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
                            <p style='margin-top: 20px;'>Please review this new ingredient.</p>
                        </div>";
        await _emailService.SendEmailAsync(subject, body);
        return ingredient.Id;
    }
}
