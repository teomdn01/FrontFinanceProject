using FrontFinanceBackend.Models;
using FrontFinanceBackend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Core.Contracts;
using Core.Models;
using Core.Models.AccountInfo;
using Core.Models.Auth;
using HtmlAgilityPack;
using Org.Front.Core.Contracts.Models.Brokers;
using TwoCaptcha.Captcha;

namespace FrontFinanceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<FrontUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        private readonly IBrokerAuthService brokerAuthService;

        [ActivatorUtilitiesConstructor]
        public AuthenticationController(IUserService userService, 
            UserManager<FrontUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IBrokerAuthService brokerAuthService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            this.brokerAuthService = brokerAuthService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage;

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Success = false, Message = "Error logging in!", Errors = errors.ToList() });
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await _userService.GenerateJwt(user);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage;

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Success = false, Message = "Error creating user!", Errors = errors.ToList() });
            }

            var userExists = await _userManager.FindByNameAsync(userDto.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Success = false,
                        Message = "Error creating user!",
                        Errors = new List<string> { "User already exists!" }
                    });

            FrontUser newUser = new()
            {
                Email = userDto.Email!,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = userDto.FirstName!,
                LastName = userDto.LastName!,
                Password = userDto.Password!,
                UserName = userDto.Email
            };

            var result = await _userManager.CreateAsync(newUser, userDto.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Success = false,
                        Message = "Error creating user!",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    });

            // add User role to the new User
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return Ok(new Response { Success = true, Message = "User created successfully!" });
        }


        [HttpGet]
        [Route("get-authenticated-user")]
        public async Task<ActionResult> getAuthenticatedUserByTokenAsync()
        {
            String email = User.FindFirst("Email")?.Value;
            if (email == null)
                return null;

            FrontUser user = await _userManager.FindByNameAsync(email);
            if (user == null)
                return null;

            UserDto userDTO = new UserDto(user);

            return StatusCode(200, userDTO);
        }
        
        [HttpGet]
        [Route("authenticate/{brokerType}")]
        public async Task<BrokerAuthPromptResponse> AuthenticateBroker(
            [FromRoute] BrokerType brokerType)
        {
            return await brokerAuthService.GetBrokerAuthLink(brokerType, new BrokerGetAuthenticationLinkRequest(){RedirectLink = $"http://localhost:3000/oauth/{brokerType.ToString().ToLower()}"}, "123423");
        }
        
        [HttpPost]
        [Route("authenticate")]
        public async Task<BrokerAuthResponse> AuthenticateBroker(
            [FromBody] BrokerAuthRequest request)
        {
            return await brokerAuthService.GetBrokerAuthResponse(request);
        }
        
        [HttpPost("token/refresh")]
        public async Task<ActionResult<BrokerAuthResponse>> RefreshToken([FromBody] BrokerRefreshTokenRequest request)
        {
            var result = await brokerAuthService.RefreshToken(request);
            return result;
        }
        
        //TODO: REFACTOR WITH SWITCHING TO UNIVERSAL POSITIONSDTO instead of string
        [HttpPost("positions")]
        public async Task<BrokerPositions> GetPositions(BrokerBaseRequest request)
        {
            var result = await brokerAuthService.GetPositions(request);
            return result;
        }
        
        //TODO: REFACTOR WITH SWITCHING TO UNIVERSAL BALANCEDTO instead of string
        [HttpPost("balance")]
        public async Task<BrokerBalance> GetBalance(BrokerBaseRequest request)
        {
            var result = await brokerAuthService.GetBalance(request);
            return result;
        }
        
    }
}