using AutenticacionService.Domain.Entities;
using AutenticacionService.Persistence.Context;
using AutenticacionService.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutenticacionService.Persistence.Repositories.Implements
{
    public class UserAtelierRepository : Repository<UserAtelier>, IUserAtelierRepository
    {
        public UserAtelierRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
