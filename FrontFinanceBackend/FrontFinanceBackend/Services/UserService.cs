﻿using FrontFinanceBackend.Models;
using FrontFinanceBackend.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace FrontFinanceBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly UserManager<FrontUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<FrontUser> userManager, IConfiguration configuration, IUserRepo userRepo)
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> GenerateJwt(FrontUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
                {
                    new Claim("Id", user.Id),
                    new Claim("Email", user.Email),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        
        public async Task<List<FrontUser>> GetAll()
        {
            return await _userRepo.GetAll();
        }
        
    }
}