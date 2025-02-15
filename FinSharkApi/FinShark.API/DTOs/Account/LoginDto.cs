using System.ComponentModel.DataAnnotations;

namespace FinShark.API.DTOs.Account;

public class LoginDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
