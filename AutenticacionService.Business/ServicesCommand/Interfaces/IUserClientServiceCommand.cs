using AutenticacionService.Domain.Models;
using System.Threading.Tasks;

namespace AutenticacionService.Business.ServicesCommand.Interfaces
{
    public interface IUserClientServiceCommand
    {
        Task<UserClientRead> Create(UserClientCreate userClientCreate);
    }
}
