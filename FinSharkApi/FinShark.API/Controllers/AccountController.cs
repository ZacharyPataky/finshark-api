using FinShark.API.DTOs;
using FinShark.API.DTOs.Account;
using FinShark.API.Helpers;
using FinShark.API.Interfaces;
using FinShark.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinShark.API.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ValidationHelpers.GetValidationErrors(ModelState);
            return BadRequest(ApiResponse<Dictionary<string, List<string>>>
                .FailureResponse("Invalid validation", errors));
        }

        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

        if (user == null)
            return Unauthorized(ApiResponse<NewUserDto>.FailureResponse("Invalid username."));

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
            return Unauthorized(ApiResponse<NewUserDto>.FailureResponse("Username not found and/or password incorrect."));

        var newUser = new NewUserDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = _tokenService.CreateToken(user)
        };
        return Ok(ApiResponse<NewUserDto>.SuccessResponse(newUser, "Succcessfully logged in."));
    }
}
