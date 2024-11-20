using Microsoft.AspNetCore.Mvc;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.DTO.UserDTO;

namespace FreshInventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IUserService userService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        if (registerUserDto == null)
        {
            _logger.LogWarning("Received null data for user registration.");
            return BadRequest(new { message = "Invalid data." }); ;
        }

        try
        {
            var result = await _userService.RegisterUserAsync(registerUserDto);

            if (result)
            {
                _logger.LogInformation("User registered successfully: {Email}", registerUserDto.Email);
                return Ok(new { message = "User registered successfully." });
            }
            else
            {
                _logger.LogWarning("User registration failed for email: {Email}", registerUserDto.Email);
                return BadRequest(new { message = "User registration failed." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user registration for email: {Email}", registerUserDto.Email);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
    {
        if (loginUserDto == null)
        {
            _logger.LogWarning("Received null data for user login.");
            return BadRequest("Invalid data.");
        }

        try
        {
            var response = await _userService.LoginUserAsync(loginUserDto);

            if (response != null)
            {
                _logger.LogInformation("User logged in successfully: {Email}", loginUserDto.Email);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("Invalid login credentials for email: {Email}", loginUserDto.Email);
                return Unauthorized("Invalid login credentials.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user login for email: {Email}", loginUserDto.Email);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
    {
        if (updateUserDto == null)
        {
            _logger.LogWarning("Invalid user update request received.");
            return BadRequest("Invalid data.");
        }

        try
        {
            var result = await _userService.UpdateUserAsync(updateUserDto);

            if (result)
            {
                _logger.LogInformation("User with ID {UserId} updated successfully.", updateUserDto.Id);
                return Ok("User updated successfully.");
            }

            _logger.LogWarning("Failed to update user with ID {UserId}.", updateUserDto.Id);
            return BadRequest("Failed to update user.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating user with ID {UserId}.", updateUserDto.Id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

}
