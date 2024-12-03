using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.DTO.UserDTO;
using FreshInventory.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace FreshInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthController(IUserService userService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("RegisterUser")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserCreateDto registerUserDto)
    {
        if (registerUserDto == null)
        {
            _logger.LogWarning("Received null data for user registration.");
            return BadRequest(new { message = "Invalid data." });
        }

        try
        {
            var result = await _userService.RegisterUserAsync(registerUserDto);

            if (result != null)
            {
                _logger.LogInformation("User registered successfully: {Email}", registerUserDto.Email);
                return Ok(new { message = "User registered successfully.", user = result });
            }

            _logger.LogWarning("User registration failed for email: {Email}", registerUserDto.Email);
            return BadRequest(new { message = "User registration failed." });
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception during user registration for email: {Email}", registerUserDto.Email);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user registration for email: {Email}", registerUserDto.Email);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpPost("LoginUser")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginUserDto)
    {
        if (loginUserDto == null)
        {
            _logger.LogWarning("Received null data for user login.");
            return BadRequest(new { message = "Invalid data." });
        }

        try
        {
            var response = await _userService.LoginUserAsync(loginUserDto);

            if (response != null)
            {
                _logger.LogInformation("User logged in successfully: {Email}", loginUserDto.Email);
                return Ok(response);
            }

            _logger.LogWarning("Invalid login credentials for email: {Email}", loginUserDto.Email);
            return Unauthorized(new { message = "Invalid login credentials." });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access attempt for email: {Email}", loginUserDto.Email);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user login for email: {Email}", loginUserDto.Email);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpPut("UpdateUserProfile")]
    public async Task<IActionResult> UpdateUser([FromQuery] string userId, [FromBody] UserUpdateDto updateUserDto)
    {
        if (updateUserDto == null)
        {
            _logger.LogWarning("Received null data for user update.");
            return BadRequest(new { message = "Invalid data." });
        }

        if (!Guid.TryParse(userId, out var userGuid))
        {
            _logger.LogWarning("Invalid user ID format received: {UserId}", userId);
            return BadRequest(new { message = "Invalid user ID format." });
        }

        try
        {
            var result = await _userService.UpdateUserAsync(userGuid, updateUserDto);

            if (result != null)
            {
                _logger.LogInformation("User with ID {UserId} updated successfully.", userGuid);
                return Ok(new { message = "User updated successfully.", user = result });
            }

            _logger.LogWarning("Failed to update user with ID {UserId}.", userGuid);
            return BadRequest(new { message = "Failed to update user." });
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception while updating user with ID {UserId}.", userGuid);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating user with ID {UserId}.", userGuid);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning("Invalid user ID received.");
            return BadRequest(new { message = "Invalid user ID." });
        }

        try
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user != null)
            {
                _logger.LogInformation("User with ID {UserId} retrieved successfully.", id);
                return Ok(user);
            }

            _logger.LogWarning("User with ID {UserId} not found.", id);
            return NotFound(new { message = "User not found." });
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception while retrieving user with ID {UserId}.", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving user with ID {UserId}.", id);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpGet("GetByEmail/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            _logger.LogWarning("Invalid email received for GetUserByEmail.");
            return BadRequest(new { message = "Invalid email." });
        }

        try
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user != null)
            {
                _logger.LogInformation("User with email {Email} retrieved successfully.", email);
                return Ok(user);
            }

            _logger.LogWarning("User with email {Email} not found.", email);
            return NotFound(new { message = "User not found." });
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception while retrieving user with email {Email}.", email);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving user with email {Email}.", email);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }
}
