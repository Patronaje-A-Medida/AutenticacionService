using AutenticacionService.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AutenticacionService.Business.ServicesCommand.Interfaces
{
    public interface IUserClientServiceCommand
    {
        Task<UserClientRead> Create(UserClientCreate userClientCreate);
        Task<UserClientRead> Update(UserClientUpdate userClientUpdate);
        Task<string> UploadImageProfile(int id, IFormFile imageFile);
    }
}
