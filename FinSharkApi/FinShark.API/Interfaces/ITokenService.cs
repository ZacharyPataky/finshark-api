using FinShark.API.Models;

namespace FinShark.API.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
