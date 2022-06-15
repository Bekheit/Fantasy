using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationalResult>>> GetUsers()
        {
            var users = await userRepository.GetUsersAsync();
            OperationalResult result = new();
            result.Content = mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OperationalResult>> GetUser(int id)
        {
            var user = await userRepository.FindByIdAsync(id);
            OperationalResult result = new();

            if (user is null)
            {
                result.BadRequest("Invalid user id");
                return BadRequest(result);
            }
            result.Content = mapper.Map<MemberDto>(user);
            return Ok(result);
        }

        [HttpGet("profile")]
        public async Task<ActionResult<OperationalResult>> GetProfile()
        {
            var user = await userRepository.FindByEmailAsync(User.GetUserEmail());
            OperationalResult result = new();
            result.Content = mapper.Map<MemberDto>(user);
            return Ok(result);
        }
    }
}
