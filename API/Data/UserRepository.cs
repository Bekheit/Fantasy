using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;

        public UserRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await context.Users
                        .Include(u => u.Photo)
                        .SingleOrDefaultAsync(u => u.Email == email );
            
        }

        public async Task<User> FindByIdAsync(int id)
        {
            return await context.Users
                    .Include(u => u.Photo)
                    .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await context.Users
                    .Include(u => u.Photo)
                    .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }


        public void Update(User user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}
