using AutenticacionService.Domain.Models;
using System.Threading.Tasks;

namespace AutenticacionService.Business.ServicesQuerys.Interfaces
{
    public interface IUserClientServiceQuery
    {
        Task<UserClientRead> SignIn(UserLogin userLogin);
    }
}
