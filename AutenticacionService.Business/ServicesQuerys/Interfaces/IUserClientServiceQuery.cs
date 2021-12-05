using AutenticacionService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutenticacionService.Business.ServicesQuerys.Interfaces
{
    public interface IUserClientServiceQuery
    {
        Task<UserClientRead> SignIn(UserLogin userLogin);
    }
}
