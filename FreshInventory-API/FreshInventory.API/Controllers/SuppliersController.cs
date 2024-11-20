using FreshInventory.Application.Common;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreshInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger) : ControllerBase
    {
        private readonly ISupplierService _supplierService = supplierService;
        private readonly ILogger<SupplierController> _logger = logger;

        [HttpGet("GetSupplierById/{id}")]
        public async Task<ActionResult<SupplierDto>> GetSupplierById(int id)
        {
            try
            {
                var supplier = await _supplierService.GetSupplierByIdAsync(id);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID {Id} not found.", id);
                    return NotFound($"Supplier with ID {id} not found.");
                }
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving supplier with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetAllSuppliers")]
        public async Task<ActionResult<PagedList<SupplierDto>>> GetAllSuppliers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? name = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = null)
        {
            try
            {
                var suppliers = await _supplierService.GetAllSuppliersAsync(pageNumber, pageSize, name, sortBy, sortDirection);
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving suppliers.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("CreateSupplier")]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierCreateDto supplierCreateDto)
        {
            try
            {
                if (supplierCreateDto == null)
                {
                    _logger.LogWarning("Invalid supplier creation data provided.");
                    return BadRequest("Invalid data.");
                }

                var createdSupplier = await _supplierService.CreateSupplierAsync(supplierCreateDto);
                return CreatedAtAction(nameof(GetSupplierById), new { id = createdSupplier.Id }, createdSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating supplier.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("UpdateSupplier/{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierUpdateDto supplierUpdateDto)
        {
            try
            {
                if (id != supplierUpdateDto.Id)
                {
                    _logger.LogWarning("ID mismatch for supplier update. URL ID: {UrlId}, DTO ID: {DtoId}.", id, supplierUpdateDto.Id);
                    return BadRequest("ID mismatch.");
                }

                await _supplierService.UpdateSupplierAsync(supplierUpdateDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating supplier with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("DeleteSupplier/{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                await _supplierService.DeleteSupplierAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting supplier with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
