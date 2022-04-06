using AutenticacionService.Domain.Entities;
using System.Threading.Tasks;

namespace AutenticacionService.Persistence.Repositories.Interfaces
{
    public interface IUserClientRepository : IRepository<UserClient>
    {
        Task<UserClient> GetByUserId(string userId);
        Task<bool> ExistUser(int id);
    }
}
