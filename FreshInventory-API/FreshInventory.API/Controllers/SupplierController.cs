using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.DTO.SupplierDTO;

namespace FreshInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger) : ControllerBase
{
    private readonly ISupplierService _supplierService = supplierService;
    private readonly ILogger<SupplierController> _logger = logger;

    [HttpPost]
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

    [HttpGet("{id}")]
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

    [HttpGet]
    public async Task<IActionResult> GetAllSuppliers()
    {
        try
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            _logger.LogInformation("Retrieved all suppliers successfully.");
            return Ok(suppliers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all suppliers.");
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
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
