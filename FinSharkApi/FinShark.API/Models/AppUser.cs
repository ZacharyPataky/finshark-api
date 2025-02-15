using Microsoft.AspNetCore.Identity;

namespace FinShark.API.Models;

public class AppUser : IdentityUser
{
    public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
}
