using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
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
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await userRepository.GetUsersAsync();
            var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetUser(int id)
        {
            var user = await userRepository.FindByIdAsync(id);
            return mapper.Map<MemberDto>(user);
        }

        [HttpGet("profile")]
        public async Task<ActionResult<MemberDto>> GetProfile()
        {
            var user = await userRepository.FindByEmailAsync(User.GetUserEmail());
            return mapper.Map<MemberDto>(user);
        }
    }
}
