using api.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AddressBook> _useManager;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration, UserManager<AddressBook> userManager, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _useManager = userManager;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(14),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LogInDto model)
        {
            var user =  _useManager.Users.FirstOrDefault(e=>e.Email==model.Email);
            if (user != null && model.Password==user.Password)
            {
                var userRoles = await _useManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Sid, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", user.Id),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    FullName = user.FullName,
                    Email = user.Email,
                });
            }
            return Unauthorized();
        }
        [HttpGet]
        [Route("GetLoginsData")]
        [Authorize]
        public async Task<IActionResult> GetUserData()
        {
            var userName = User?.Identity?.Name;
            var userId = User?.FindFirstValue(ClaimTypes.Sid);
            List<Claim> roleClaims = User?.FindAll(ClaimTypes.Role).ToList();
            var roles = new List<string>();

            foreach (var role in roleClaims)
            {
                roles.Add(role.Value);
            }
            return Ok(new { roles, userName, userId });
        }

    }
}
