using AutenticacionService.Domain.Entities;
using AutenticacionService.Domain.Utils;
using AutenticacionService.Persistence.Context;
using AutenticacionService.Persistence.Handlers;
using AutenticacionService.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static AutenticacionService.Domain.Utils.ErrorsUtil;

namespace AutenticacionService.Persistence.Repositories.Implements
{
    public class UserAtelierRepository : Repository<UserAtelier>, IUserAtelierRepository
    {
        public UserAtelierRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<UserAtelier> GetByUserId_Role(string userId, string role)
        {
            try
            {
                var result = await _context.UserAteliers
                .AsNoTracking()
                .Include(u => u.User)
                .Where(u => u.UserId.Equals(userId))
                .Where(u => u.User.Role.Equals(role))
                .Where(u => u.Status && u.User.Status.Equals(StatusUtil.USER_ACTIVE))
                .FirstOrDefaultAsync();

                return result;
            }
            catch(Exception ex)
            {
                throw new RepositoryException(
                    HttpStatusCode.InternalServerError,
                    ErrorsCode.GET_USER_ERROR,
                    ErrorMessages.GET_USER_ERROR,
                    ex);
            }
        }
    }
}
