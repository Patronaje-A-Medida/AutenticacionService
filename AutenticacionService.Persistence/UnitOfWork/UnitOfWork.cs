using AutenticacionService.Persistence.Context;
using AutenticacionService.Persistence.Repositories.Implements;
using AutenticacionService.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutenticacionService.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _context;
        public IUserClientRepository userClientRepository { get; private set; }
        public IUserAtelierRepository userAtelierRepository { get; private set; }

        public UnitOfWork(AuthDbContext context)
        {
            _context = context;
            userClientRepository = new UserClientRepository(_context);
            userAtelierRepository = new UserAtelierRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
