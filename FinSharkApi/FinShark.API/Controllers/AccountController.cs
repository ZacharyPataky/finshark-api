using FinShark.API.DTOs;
using FinShark.API.DTOs.Account;
using FinShark.API.Helpers;
using FinShark.API.Interfaces;
using FinShark.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.API.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ValidationHelpers.GetValidationErrors(ModelState);
                return BadRequest(ApiResponse<Dictionary<string, List<string>>>
                    .FailureResponse("Invalid validation", errors));
            }

            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.EmailAddress,
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                    return Ok(ApiResponse<NewUserDto>.SuccessResponse(new NewUserDto
                    {
                        UserName = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    }));
            }

            return BadRequest(ApiResponse<string>.FailureResponse("The user was not created.", createdUser.Errors.Select(error => error.Description).ToList()));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.FailureResponse("The user was not created."));
        }
    }
}
