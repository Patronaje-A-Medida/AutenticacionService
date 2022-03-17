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
    public class UserClientRepository : Repository<UserClient>, IUserClientRepository
    {
        public UserClientRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<UserClient> GetByUserId(string userId)
        {
            try
            {
                var result = await _context.UserClients
                .AsNoTracking()
                .Include(u => u.User)
                .Where(u => u.UserId.Equals(userId))
                .Where(u => u.User.Role.Equals(RolesUtil.CLIENT))
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
