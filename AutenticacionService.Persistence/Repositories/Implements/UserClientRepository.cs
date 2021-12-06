using AutenticacionService.Domain.Entities;
using AutenticacionService.Domain.Utils;
using AutenticacionService.Persistence.Context;
using AutenticacionService.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutenticacionService.Persistence.Repositories.Implements
{
    public class UserClientRepository : Repository<UserClient>, IUserClientRepository
    {
        public UserClientRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<UserClient> GetByUserId(string userId)
        {
            var result = await _context.UserClients
                .Include(u => u.User)
                .Where(u => u.UserId.Equals(userId))
                .Where(u => u.User.Role.Equals(RolesUtil.CLIENT))
                .Where(u => u.Status && u.User.Status.Equals(StatusUtil.USER_ACTIVE))
                .FirstOrDefaultAsync();

            if (result == null) throw new Exception("error repo user null");

            return result;
        }
    }
}
