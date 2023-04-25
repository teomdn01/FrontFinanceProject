using FrontFinanceBackend.Models;
using FrontFinanceBackend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Core.Contracts;
using Core.Models;
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
            return await brokerAuthService.GetBrokerAuthLink(brokerType, new BrokerGetAuthenticationLinkRequest(){RedirectLink = "http://localhost:8080/"}, "123423");
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
        
        // [HttpPost]
        // [Route("2captcha")]
        // public async Task<string> SolveCaptcha()
        // {
        //     var apiKey = "500fc6cc55eb61a565e65a4ecd18a374";
        //     var solver = new TwoCaptcha.TwoCaptcha(apiKey)
        //     {
        //         DefaultTimeout = 120,
        //         RecaptchaTimeout = 600,
        //         PollingInterval = 10
        //     };
        //
        //    // HCaptcha captcha = new HCaptcha();
        //    // captcha.SetSiteKey("55358dd0-6380-4e69-8390-647a403a8a7f"); //mobile-bitstamp
        //     
        //    // hCaptcha.SetSiteKey("55358dd0-6380-4e69-8390-647a403a8a7f"); //web-bitstamp
        //    // captcha.SetSiteKey("0ae3f87c-8d4d-44b0-bb90-2370959e8438"); //mobile-kucoin
        //    // captcha.SetUrl("https://www.bitstamp.net/onboarding/login/"); //bitstamp
        //    // captcha.SetUrl("https://0ae3f87c-8d4d-44b0-bb90-2370959e8438.android-sdk.hcaptcha.com"); //kucoin
        //     
        //     //kucoin geetest
        //     // GeeTest captcha = new GeeTest();
        //     // captcha.SetGt("7d6a1e6783aa2d942687f7e11129b6be"); 
        //     // //captcha.SetApiServer("api.geetest.com");
        //     // captcha.SetChallenge("65433dcbb1b386eb26fc4cf2785e827f");
        //     
        //     
        //     // GeeTestV4 captcha = new GeeTestV4();
        //     // //eu f5c2ad5a8a3cf37192d8b9c039950f79
        //     // // 
        //     // captcha.SetCaptchaId("4ad0e180967a9303c84bfff9a700b958");
        //     // captcha.SetUrl("https://www.bitget.com/en/login?from=%2Fen%2Fsupport&source=cms");
        //     
        //     
        //     //captcha.SetUrl("https://2captcha.com/demo/geetest");
        //     
        //     
        //     //RECAPTCHA
        //     
        //     //6LdbJ_YUAAAAALotZ5cQcc-1v1tbaAHjevfYqTxf Cex.io web
        //     
        //     //
        //     
        //     //reCAPTCHA captcha = new reCAPTCHA();
        //     Console.WriteLine("Start Bitstamp Mobile");
        //     // var captcha = new ReCaptcha();
        //     // captcha.SetSiteKey("6LcnUrIcAAAAAAQWVwzP_p7G32z4ZBs4D9-0XvEM");
        //     // captcha.SetUrl("https://login.coinbase.com/signin");
        //     try
        //     {
        //         await solver.Solve(captcha);
        //         Console.WriteLine("Captcha solved: " + captcha.Code);
        //     }
        //     catch (AggregateException e)
        //     {
        //         Console.WriteLine("Error occurred: " + e.Message);
        //     }
        //
        //     return captcha.Code;
        // }
        
    }
}