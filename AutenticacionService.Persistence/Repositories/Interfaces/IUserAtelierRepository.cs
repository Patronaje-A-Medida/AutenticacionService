using AutenticacionService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutenticacionService.Persistence.Repositories.Interfaces
{
    public interface IUserAtelierRepository : IRepository<UserAtelier>
    {
        Task<UserAtelier> GetByUserId_Role(string userId, string role);
        Task<IEnumerable<UserAtelier>> GetUsersByAtelier(int atelierId);

    }
}
