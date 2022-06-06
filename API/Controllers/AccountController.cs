using API.Data;
using API.DTOs;
using API.Entities;
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
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            using var hmac = new HMACSHA512();
            var user = new User
            {
                UserName = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Username)),
                PasswordSalt = hmac.Key,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                TeamName = registerDto.TeamName,
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDto
            {
                Email = registerDto.Email,
                Token = tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginDto loginDto)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null) return Unauthorized("Invalid Email");

            return user;

            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));



            for(int i=0; i<computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid Password");
            }

            //return new UserDto
            //{
            //    Email = loginDto.Email,
            //    Token = tokenService.CreateToken(user)
            //};
        }
    }
}
