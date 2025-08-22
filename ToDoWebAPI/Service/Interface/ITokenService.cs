using System.Security.Claims;
using ToDoWebAPI.Data.Models;
using static ToDoWebAPI.Dtos.TokenDtos;

namespace ToDoWebAPI.Service.Interface
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user, List<string> roles);
        RefreshTokenDto GenerateRrefreshToken();

        ClaimsPrincipal GetClaimsPrincipal(string token);
    }
}
