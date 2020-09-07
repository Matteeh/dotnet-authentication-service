
using identity.Models;

namespace identity.Services
{
    public interface ITokenBuilder
    {
        string BuildToken(ApplicationUser appUser);
    }
}