using WorkBuddy.Api.Entities;

namespace WorkBuddy.Api.Interfaces
{
    public interface ITokenService
    {

        string CreateToken(AppUser user);



    }
}
