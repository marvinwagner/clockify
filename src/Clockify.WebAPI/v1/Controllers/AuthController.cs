using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Clockify.Auth.WebAPI.Extensions;
using Clockify.Auth.WebAPI.ViewModel;
using Clockify.Core.Messages.Notifications;
using Clockify.WebAPI.Controllers;
using KissLog;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Clockify.WebAPI.v1.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : BaseController
    {
        //private readonly ILogger<AuthController> _logger;
        private readonly ILogger _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(INotificationHandler<DomainNotification> notifications, ILogger logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<AppSettings> appSettings)
                    : base(notifications)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        private async Task<LoginResponseViewModel> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            //Needed to get userId
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new LoginResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserTokenViewModel
                {
                    Id = user.Id,
                    Email = user.Email
                }
            };

            return response;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserViewModel userModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(userModel);

            var user = new IdentityUser
            {
                UserName = userModel.Email,
                Email = userModel.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok(await GenerateJwt(userModel.Email));
            }
            return BadRequest(result.Errors.Select(x => x.Description));
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserViewModel userModel)
        {
            _logger.Info($"The method {GetType().Name} was called.");

            if (!ModelState.IsValid)
                return BadRequest(userModel);

            var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, false, true);
            if (result.Succeeded)
                return Ok(await GenerateJwt(userModel.Email));

            if (result.IsLockedOut)
                return BadRequest("User temporary blocked");

            return BadRequest("Invalid user or password");
        }
    }
}
