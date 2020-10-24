//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using Clockify.Auth.WebAPI.Extensions;
//using Clockify.Auth.WebAPI.ViewModel;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;

//namespace Clockify.Auth.WebAPI.Controllers
//{
//    [ApiController]
//    //[Route("api/[controller]")]
//    public class AuthController : ControllerBase
//    {
//        private readonly ILogger<AuthController> _logger;
//        private readonly SignInManager<IdentityUser> _signInManager;
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly AppSettings _appSettings;

//        public AuthController(ILogger<AuthController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<AppSettings> appSettings)
//        {
//            _logger = logger;
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _appSettings = appSettings.Value;
//        }

//        private async Task<LoginResponseViewModel> GenerateJwt(string email)
//        {
//            var user = await _userManager.FindByEmailAsync(email);
//            var claims = await _userManager.GetClaimsAsync(user);
//            var roles = await _userManager.GetRolesAsync(user);

//            // Adicionamos as Claims padrões do token, apenas para garantir que está sendo gerado
//            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
//            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
//            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
//            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
//            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
//            foreach (var userRole in roles)
//            {
//                claims.Add(new Claim("role", userRole));
//            }

//            var identityClaims = new ClaimsIdentity();
//            identityClaims.AddClaims(claims);

//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
//            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
//            {
//                Issuer = _appSettings.Emissor,
//                Audience = _appSettings.ValidoEm,
//                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
//                Subject = identityClaims
//            });

//            var encodedToken = tokenHandler.WriteToken(token);

//            var response = new LoginResponseViewModel
//            {
//                AccessToken = encodedToken,
//                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
//                UserToken = new UserTokenViewModel
//                {
//                    Id = user.Id,
//                    Email = user.Email,
//                    Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
//                }
//            };

//            return response;
//        }

//        // retorna os segundos relativos a data que está sendo passada
//        private static long ToUnixEpochDate(DateTime date)
//            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

//        /* VERSAO PARA SÓ VALIDAR TOKEN, SEM CLAIM NEM ROLES
//        private async Task<string> GenerateJwt()
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
//            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
//            {
//                Issuer = _appSettings.Emissor,
//                Audience = _appSettings.ValidoEm,
//                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//            });

//            var encodedToken = tokenHandler.WriteToken(token);

//            return encodedToken;
//        }
//        */
//        [HttpPost("register")]
//        public async Task<IActionResult> Register(RegisterUserViewModel userModel)
//        {
//            if (!ModelState.IsValid) 
//                return BadRequest(userModel);

//            var user = new IdentityUser
//            {
//                UserName = userModel.Email,
//                Email = userModel.Email,
//                EmailConfirmed = true
//            };

//            var result = await _userManager.CreateAsync(user, userModel.Password);
//            if (result.Succeeded)
//            {
//                await _signInManager.SignInAsync(user, false);
//                return Ok(await GenerateJwt(userModel.Email));
//            }
//            return BadRequest(result.Errors.Select(x => x.Description));
//        }


//        [HttpPost("login")]
//        public async Task<IActionResult> Login(LoginUserViewModel userModel)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(userModel);

//            var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, false, true);
//            if (result.Succeeded)
//                return Ok(await GenerateJwt(userModel.Email));

//            if (result.IsLockedOut)
//                return BadRequest("User temporary blocked");

//            return BadRequest("Invalid user or password");
//        }
//    }
//}
