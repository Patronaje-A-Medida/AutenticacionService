using AutenticacionService.Domain.Models;
using System.Threading.Tasks;

namespace AutenticacionService.Business.ServicesCommand.Interfaces
{
    public interface IUserAtelierServiceCommand
    {
        Task<UserAtelierRead> CreateOwner(UserOwnerCreate userOwnerCreate);
        Task<UserAtelierRead> CreateTechnician(UserTechnicianCreate userTechnicianCreate);
    }
}
