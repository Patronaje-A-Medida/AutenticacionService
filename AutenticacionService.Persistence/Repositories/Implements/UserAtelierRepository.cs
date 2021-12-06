using AutenticacionService.Domain.Entities;
using AutenticacionService.Persistence.Context;
using AutenticacionService.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutenticacionService.Persistence.Repositories.Implements
{
    public class UserAtelierRepository : Repository<UserAtelier>, IUserAtelierRepository
    {
        public UserAtelierRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<UserAtelier> GetByUserId_Role(string userId, string role)
        {
            var result = await _context.UserAteliers
                .Include(u => u.User)
                .Where(u => u.UserId.Equals(userId))
                .Where(u => u.User.Role.Equals(role))
                .FirstOrDefaultAsync();

            if (result == null) throw new Exception("error repo user null");

            return result;
        }
    }
}
