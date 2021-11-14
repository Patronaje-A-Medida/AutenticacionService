using AutenticacionService.Domain.Entities;
using AutenticacionService.Persistence.Context;
using AutenticacionService.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutenticacionService.Persistence.Repositories.Implements
{
    public class UserClientRepository : Repository<UserClient>, IUserClientRepository
    {
        public UserClientRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
