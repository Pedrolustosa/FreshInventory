﻿using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.DTO.SupplierDTO;
using Microsoft.AspNetCore.Authorization;

namespace FreshInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger) : ControllerBase
{
    private readonly ISupplierService _supplierService = supplierService;
    private readonly ILogger<SupplierController> _logger = logger;

    [HttpPost("Create")]
    public async Task<IActionResult> CreateSupplier([FromBody] SupplierCreateDto supplierDto)
    {
        if (supplierDto == null)
        {
            _logger.LogWarning("Received null data for supplier creation.");
            return BadRequest(new { message = "Invalid data." });
        }

        try
        {
            var createdSupplier = await _supplierService.CreateSupplierAsync(supplierDto);
            _logger.LogInformation("Supplier created successfully with ID: {Id}", createdSupplier.Id);
            return CreatedAtAction(nameof(GetSupplierById), new { id = createdSupplier.Id }, createdSupplier);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the supplier.");
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetSupplierById(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Invalid supplier ID received: {Id}", id);
            return BadRequest(new { message = "Invalid supplier ID." });
        }

        try
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID {Id} not found.", id);
                return NotFound(new { message = "Supplier not found." });
            }

            _logger.LogInformation("Supplier with ID {Id} retrieved successfully.", id);
            return Ok(supplier);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving supplier with ID {Id}.", id);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpGet("GetAllPaged")]
    public async Task<IActionResult> GetAllSuppliersPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var paginatedSuppliers = await _supplierService.GetAllSuppliersPagedAsync(pageNumber, pageSize);
            _logger.LogInformation("Successfully retrieved paginated suppliers. Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
            return Ok(paginatedSuppliers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving paginated suppliers.");
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierUpdateDto supplierDto)
    {
        if (supplierDto == null)
        {
            _logger.LogWarning("Received null data for supplier update.");
            return BadRequest(new { message = "Invalid data." });
        }

        try
        {
            var updatedSupplier = await _supplierService.UpdateSupplierAsync(id, supplierDto);
            if (updatedSupplier == null)
            {
                _logger.LogWarning("Failed to update supplier with ID {Id}.", id);
                return NotFound(new { message = "Supplier not found." });
            }

            _logger.LogInformation("Supplier with ID {Id} updated successfully.", id);
            return Ok(updatedSupplier);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating supplier with ID {Id}.", id);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Invalid supplier ID received: {Id}", id);
            return BadRequest(new { message = "Invalid supplier ID." });
        }

        try
        {
            var isDeleted = await _supplierService.DeleteSupplierAsync(id);
            if (!isDeleted)
            {
                _logger.LogWarning("Supplier with ID {Id} could not be deleted or does not exist.", id);
                return NotFound(new { message = "Supplier not found or could not be deleted." });
            }

            _logger.LogInformation("Supplier with ID {Id} deleted successfully.", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting supplier with ID {Id}.", id);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }
}
