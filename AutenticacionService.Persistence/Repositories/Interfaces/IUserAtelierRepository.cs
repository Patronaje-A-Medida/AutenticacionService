using AutenticacionService.Domain.Entities;
using System.Threading.Tasks;

namespace AutenticacionService.Persistence.Repositories.Interfaces
{
    public interface IUserAtelierRepository : IRepository<UserAtelier>
    {
        Task<UserAtelier> GetByUserId_Role(string userId, string role);
    }
}
