using AutenticacionService.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutenticacionService.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserClientRepository userClientRepository { get; }
        IUserAtelierRepository userAtelierRepository { get; }

        Task SaveChangesAsync();
    }
}
