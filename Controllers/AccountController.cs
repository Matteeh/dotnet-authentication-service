using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using identity.Models;
using identity.Data;
using Microsoft.AspNetCore.Identity;
using identity.ViewModels;
using AutoMapper;
using identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private AppDbContext _context;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ITokenBuilder tokenBuilder, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenBuilder = tokenBuilder;
            _mapper = mapper;
        }

        [HttpGet("test")]
        public IActionResult test()
        {
            return Ok("OK");
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpVM user)
        {
            ApplicationUser appUser = _mapper.Map<UserSignUpVM, ApplicationUser>(user);
            appUser.UserName = user.Email;
            IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
            if (result == IdentityResult.Success)
            {
                await _signInManager.SignInAsync(appUser, false);
                var token = _tokenBuilder.BuildToken(appUser);
                return Ok(token);
            }
            else
            {
                return BadRequest("User already exists");
            }
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInVM user)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);
                if (result == Microsoft.AspNetCore.Identity.SignInResult.Success)
                {
                    var appUser = _unitOfWork.Users.Find(u => u.Email == user.Email).FirstOrDefault();
                    // var appUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                    var token = _tokenBuilder.BuildToken(appUser);
                    return Ok(token);
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }

        }

        [HttpGet("verify")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> VerifyToken()
        {
            var appUserEmail = User
                .Claims
                .First(c => c.Type == "Email").Value;

            if (appUserEmail == null)
            {
                return Unauthorized();
            }
            var appUserExists = _unitOfWork.Users.Find(u => appUserEmail == u.Email).FirstOrDefault();

            if (appUserExists == null)
            {
                return Unauthorized();
            }

            return NoContent();
        }

        /*
                private string GenerateAccessToken(ApplicationUser appUser)
                {
                    Claim[] claims = new Claim[]{
                        new Claim(JwtRegisteredClaimNames.Sub, appUser.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("Email", appUser.Email),
                        new Claim("FirstName", appUser.FirstName),
                        new Claim("LastName", appUser.LastName)
                    };
                    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")));
                    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
                    {
                        Audience = Environment.GetEnvironmentVariable("AUDIENCE"),
                        Issuer = Environment.GetEnvironmentVariable("ISSUER"),
                        IssuedAt = DateTime.UtcNow,
                        Expires = DateTime.UtcNow.AddHours(12),
                        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                        Subject = new ClaimsIdentity(claims)
                    };
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    var token = handler.CreateToken(tokenDescriptor);
                    return handler.WriteToken(token);

                }

         */
    }
}
