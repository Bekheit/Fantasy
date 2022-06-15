using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.context = context;
            this.tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<OperationalResult>> Register(RegisterDto registerDto)
        {
            OperationalResult result = new();

            if(await UserExist(registerDto.Email))
            {
                result.BadRequest("Email already exist");
                return result;
            }
            using var hmac = new HMACSHA512();
            var user = new User
            {
                UserName = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                TeamName = registerDto.TeamName,
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            result.Content = new UserDto
            {
                Email = registerDto.Email,
                Token = tokenService.CreateToken(user)
            };

            return result;
        }

        [HttpPost("login")]
        public async Task<ActionResult<OperationalResult>> Login(LoginDto loginDto)
        {
            try
            {
                var x = "asd";
                if (!ModelState.IsValid)
                    x = "error";
                var user = await context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);

                if (user == null)
                    return InvalidEmailOrPassword();

                var hmac = new HMACSHA512(user.PasswordSalt);

                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

                for (int i = 0; i < computedHash.Length; i++)
                    if (computedHash[i] != user.PasswordHash[i])
                        return InvalidEmailOrPassword();

                OperationalResult result = new();

                result.Content = new UserDto
                {
                    Email = loginDto.Email,
                    Token = tokenService.CreateToken(user)
                };

                return result;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private BadRequestObjectResult InvalidEmailOrPassword()
        {
            OperationalResult result = new();
            result.BadRequest("Invalid Email or Password");
            return BadRequest(result);
        }

        private async Task<bool> UserExist(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);

        }
    }
}
