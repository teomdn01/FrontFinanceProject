using FrontFinanceBackend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FrontFinanceBackend.Models;

namespace FrontFinanceBackend.Services
{
    public interface IUserService
    {

        public Task<JwtSecurityToken> GenerateJwt(FrontUser user);

        public Task<List<FrontUser>> GetAll();

    }
}