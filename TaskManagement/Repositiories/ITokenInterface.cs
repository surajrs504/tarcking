using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Repositiories
{
    public interface ITokenInterface
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
