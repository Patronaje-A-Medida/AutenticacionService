using AutenticacionService.Domain.Models;
using System.Threading.Tasks;

namespace AutenticacionService.Business.ServicesQuerys.Interfaces
{
    public interface IUserAtelierServiceQuery
    {
        Task<UserAtelierRead> SignIn(UserLogin userLogin);
    }
}
