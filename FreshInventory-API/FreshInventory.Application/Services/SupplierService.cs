﻿using AutoMapper;
using FreshInventory.Application.CQRS.Supplier.Commands;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Features.Suppliers.Commands;
using FreshInventory.Application.Features.Suppliers.Queries;
using FreshInventory.Application.Interfaces;
using FreshInventory.Domain.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Services;

public class SupplierService(IMediator mediator, IMapper mapper, ILogger<SupplierService> logger) : ISupplierService
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<SupplierService> _logger = logger;

    public async Task<SupplierReadDto> CreateSupplierAsync(SupplierCreateDto supplierCreateDto)
    {
        if (supplierCreateDto == null)
        {
            _logger.LogWarning("Received null data for supplier creation.");
            throw new ArgumentNullException(nameof(supplierCreateDto), "SupplierCreateDto cannot be null.");
        }

        try
        {
            _logger.LogInformation("Starting supplier creation for: {SupplierName}", supplierCreateDto.Name);

            var command = _mapper.Map<CreateSupplierCommand>(supplierCreateDto);
            var supplierReadDto = await _mediator.Send(command);

            _logger.LogInformation("Supplier created successfully with ID: {SupplierId}", supplierReadDto.Id);
            return supplierReadDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating supplier: {SupplierName}", supplierCreateDto.Name);
            throw;
        }
    }

    public async Task<SupplierReadDto> UpdateSupplierAsync(int supplierId, SupplierUpdateDto supplierUpdateDto)
    {
        if (supplierUpdateDto == null)
        {
            _logger.LogWarning("Received null data for supplier update.");
            throw new ArgumentNullException(nameof(supplierUpdateDto), "SupplierUpdateDto cannot be null.");
        }

        try
        {
            _logger.LogInformation("Starting update for supplier with ID: {SupplierId}", supplierId);

            var command = new UpdateSupplierCommand(supplierId, supplierUpdateDto);
            var updatedSupplierReadDto = await _mediator.Send(command);

            _logger.LogInformation("Supplier with ID: {SupplierId} updated successfully.", supplierId);
            return updatedSupplierReadDto;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Supplier with ID: {SupplierId} not found during update.", supplierId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating supplier with ID: {SupplierId}.", supplierId);
            throw;
        }
    }

    public async Task<SupplierReadDto> GetSupplierByIdAsync(int supplierId)
    {
        if (supplierId <= 0)
        {
            _logger.LogWarning("Invalid supplier ID received: {SupplierId}", supplierId);
            throw new ArgumentException("Supplier ID must be greater than zero.", nameof(supplierId));
        }

        try
        {
            _logger.LogInformation("Retrieving supplier with ID: {SupplierId}", supplierId);

            var query = new GetSupplierByIdQuery(supplierId);
            var supplier = await _mediator.Send(query);

            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID: {SupplierId} not found.", supplierId);
                throw new KeyNotFoundException($"Supplier with ID: {supplierId} not found.");
            }

            _logger.LogInformation("Supplier with ID: {SupplierId} retrieved successfully.", supplierId);
            return supplier;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving supplier with ID: {SupplierId}.", supplierId);
            throw;
        }
    }

    public async Task<PaginatedList<SupplierReadDto>> GetAllSuppliersPagedAsync(int pageNumber, int pageSize)
    {
        try
        {
            _logger.LogInformation("Retrieving paginated suppliers. Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

            var query = new GetAllSuppliersPagedQuery(pageNumber, pageSize);
            var paginatedSuppliers = await _mediator.Send(query);

            _logger.LogInformation("Successfully retrieved paginated suppliers. Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
            return paginatedSuppliers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving paginated suppliers.");
            throw;
        }
    }


    public async Task<bool> DeleteSupplierAsync(int supplierId)
    {
        if (supplierId <= 0)
        {
            _logger.LogWarning("Invalid supplier ID received: {SupplierId}", supplierId);
            throw new ArgumentException("Supplier ID must be greater than zero.", nameof(supplierId));
        }

        try
        {
            _logger.LogInformation("Attempting to deactivate supplier with ID: {SupplierId}", supplierId);

            var command = new DeleteSupplierCommand(supplierId);
            var result = await _mediator.Send(command);

            if (result)
            {
                _logger.LogInformation("Supplier with ID: {SupplierId} deactivated successfully.", supplierId);
            }
            else
            {
                _logger.LogWarning("Failed to deactivate supplier with ID: {SupplierId}.", supplierId);
            }

            return result;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Supplier with ID: {SupplierId} not found during deactivation.", supplierId);
            throw;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Supplier with ID: {SupplierId} cannot be deactivated due to linked dependencies.", supplierId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deactivating supplier with ID: {SupplierId}.", supplierId);
            throw;
        }
    }
}
