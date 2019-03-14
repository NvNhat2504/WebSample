using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebSample_API.Data;
using WebSample_API.Dtos;
using WebSample_API.Models;

namespace WebSample_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository authRepo, IConfiguration config)
        {
            _config = config;
            _authRepo = authRepo;
        }
        // GET api/values
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userR)
        {
            if (await _authRepo.UserExists(userR.UserName))
                return BadRequest("User already exists! Please input UserName other.");
            var userInsert = new User
            {
                UserName = userR.UserName
            };
            var userCreated = await _authRepo.Register(userInsert, userR.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(UserForLoginDto userL)
        {
            var userLogin = await _authRepo.Login(userL.UserName, userL.Password);
            if (userLogin == null) return Unauthorized();
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, userLogin.Id.ToString()),
                new Claim(ClaimTypes.Name, userLogin.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSetting:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new {
               token = tokenHandler.WriteToken(token)
            });
        }
    }
}