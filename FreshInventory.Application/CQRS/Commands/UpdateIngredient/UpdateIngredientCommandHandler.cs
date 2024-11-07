using MediatR;
using AutoMapper;
using FreshInventory.Application.DTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;
using FreshInventory.Application.Interfaces;

namespace FreshInventory.Application.CQRS.Commands.UpdateIngredient;

public class UpdateIngredientCommandHandler(IIngredientRepository repository, IMapper mapper, IEmailService emailService) : IRequestHandler<UpdateIngredientCommand>
{
    private readonly IMapper _mapper = mapper;
    private readonly IEmailService _emailService = emailService;
    private readonly IIngredientRepository _repository = repository;

    public async Task Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
    {
        var existingIngredient = await _repository.GetByIdAsync(request.Ingredient.Id)
                                  ?? throw new ServiceException($"Ingredient with ID {request.Ingredient.Id} not found.");
        var originalValues = new IngredientOldValuesDto
        {
            Id = existingIngredient.Id,
            Name = existingIngredient.Name,
            Quantity = existingIngredient.Quantity,
            Unit = existingIngredient.Unit.ToString(),
            UnitCost = existingIngredient.UnitCost,
            Category = existingIngredient.Category.ToString(),
            Supplier = existingIngredient.Supplier,
            PurchaseDate = existingIngredient.PurchaseDate,
            ExpiryDate = existingIngredient.ExpiryDate,
            IsPerishable = existingIngredient.IsPerishable,
            ReorderLevel = existingIngredient.ReorderLevel
        };
        _mapper.Map(request.Ingredient, existingIngredient);
        await _repository.UpdateAsync(existingIngredient);
        var subject = $"Ingredient Updated: {existingIngredient.Name}";
        var body = $@"
                <div style='font-family: Arial, sans-serif; color: #333;'>
                    <h2 style='color: #FF9800;'>Ingredient Updated</h2>
                    <p>The following ingredient has been updated. Changes are highlighted below:</p>
                    <table style='width: 100%; border-collapse: collapse;'>
                        <tr>
                            <td style='font-weight: bold; padding: 8px; border: 1px solid #ddd;'>Field</td>
                            <td style='font-weight: bold; padding: 8px; border: 1px solid #ddd;'>Old Value</td>
                            <td style='font-weight: bold; padding: 8px; border: 1px solid #ddd;'>New Value</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Name</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{originalValues.Name}</td>
                            <td style='padding: 8px; border: 1px solid #ddd; {(originalValues.Name != existingIngredient.Name ? "color: #FF9800;" : "")}'>{existingIngredient.Name}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Quantity</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{originalValues.Quantity}</td>
                            <td style='padding: 8px; border: 1px solid #ddd; {(originalValues.Quantity != existingIngredient.Quantity ? "color: #FF9800;" : "")}'>{existingIngredient.Quantity}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Unit</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{originalValues.Unit}</td>
                            <td style='padding: 8px; border: 1px solid #ddd; {(originalValues.Unit.ToString() != existingIngredient.Unit.ToString() ? "color: #FF9800;" : "")}'>{existingIngredient.Unit}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Unit Cost</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{originalValues.UnitCost:C}</td>
                            <td style='padding: 8px; border: 1px solid #ddd; {(originalValues.UnitCost != existingIngredient.UnitCost ? "color: #FF9800;" : "")}'>{existingIngredient.UnitCost:C}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Category</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{originalValues.Category}</td>
                            <td style='padding: 8px; border: 1px solid #ddd; {(originalValues.Category.ToString() != existingIngredient.Category.ToString() ? "color: #FF9800;" : "")}'>{existingIngredient.Category}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Supplier</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{originalValues.Supplier}</td>
                            <td style='padding: 8px; border: 1px solid #ddd; {(originalValues.Supplier != existingIngredient.Supplier ? "color: #FF9800;" : "")}'>{existingIngredient.Supplier}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Purchase Date</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{originalValues.PurchaseDate:dd/MM/yyyy}</td>
                            <td style='padding: 8px; border: 1px solid #ddd; {(originalValues.PurchaseDate != existingIngredient.PurchaseDate ? "color: #FF9800;" : "")}'>{existingIngredient.PurchaseDate:yyyy-MM-dd}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Expiry Date</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{originalValues.ExpiryDate:dd/MM/yyyy}</td>
                            <td style='padding: 8px; border: 1px solid #ddd; {(originalValues.ExpiryDate != existingIngredient.ExpiryDate ? "color: #FF9800;" : "")}'>{existingIngredient.ExpiryDate:yyyy-MM-dd}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Is Perishable</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{originalValues.IsPerishable}</td>
                            <td style='padding: 8px; border: 1px solid #ddd; {(originalValues.IsPerishable != existingIngredient.IsPerishable ? "color: #FF9800;" : "")}'>{existingIngredient.IsPerishable}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Reorder Level</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{originalValues.ReorderLevel}</td>
                            <td style='padding: 8px; border: 1px solid #ddd; {(originalValues.ReorderLevel != existingIngredient.ReorderLevel ? "color: #FF9800;" : "")}'>{existingIngredient.ReorderLevel}</td>
                        </tr>
                    </table>
                    <p style='margin-top: 20px;'>Please review the changes to this ingredient.</p>
                </div>";

        await _emailService.SendEmailAsync(subject, body);
    }
}