using AutenticacionService.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutenticacionService.Business.ServicesQuerys.Interfaces
{
    public interface IUserAtelierServiceQuery
    {
        Task<UserAtelierRead> SignIn(UserLogin userLogin);
        Task<ICollection<UserAtelierRead>> GetTechniciansbyAtilierId(int atelierId);
    }
}
