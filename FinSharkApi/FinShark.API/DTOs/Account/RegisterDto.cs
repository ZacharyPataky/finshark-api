using System.ComponentModel.DataAnnotations;

namespace FinShark.API.DTOs.Account;

public class RegisterDto
{
    [Required(ErrorMessage = "You must provide a username.")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "You must provide an email address.")]
    [EmailAddress(ErrorMessage = "You must provide a properly formatted email address.")]
    public string? EmailAddress { get; set; }
    [Required(ErrorMessage = "You must provide a password.")]
    public string? Password { get; set; }
}
