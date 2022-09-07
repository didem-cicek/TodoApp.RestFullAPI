using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApp.DTOs;
using TodoApp.RestFullAPI.Models;

namespace TodoApp.RestFullAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class SecurityController : ControllerBase
    {
        private readonly TodoDbContext context;
        private readonly IConfiguration configuration;

        public SecurityController(TodoDbContext _context, IConfiguration _configuration)
        {
            context = _context;
            this.configuration = _configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RequestToken(RequestTokenDTO dto)
        {
            var appUser = await context.AppUsers.FirstOrDefaultAsync(o => o.UserName == dto.Username && o.Password == dto.Password);
            if (appUser == null)
                return BadRequest("Kullanıcı adı veya şifresi hatalı!");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, appUser.Name),
                new Claim(ClaimTypes.Surname, appUser.Surname),
                new Claim(Constants.UserId, appUser.AppUserId.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
                issuer: Constants.Issuer,
                audience: Constants.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return Ok(new TokenResponseDTO { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpGet]
        public IActionResult Test()
        {
            var nameSurname = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value
            + " "
            + User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value;

            var userId = User.Claims.FirstOrDefault(c => c.Type == Constants.UserId).Value;


            return Ok($"Yetkili erişiminiz {userId} id'li {nameSurname} kullanıcısı için var");
        }
    }
}
