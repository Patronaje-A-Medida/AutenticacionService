using AutenticacionService.Business.ServicesQuerys.Interfaces;
using AutenticacionService.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutenticacionService.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/user-atelier")]
    [Produces("application/json")]
    public class UserAtelierController : ControllerBase
    {

        private readonly IUserAtelierServiceQuery _userAtelierServiceQuery;

        public UserAtelierController(IUserAtelierServiceQuery userAtelierServiceQuery)
        {
            _userAtelierServiceQuery = userAtelierServiceQuery;
        }

        [HttpGet("{atelierId}")]
        [ProducesResponseType(typeof(ICollection<UserAtelierRead>), 200)]
        public async Task<ICollection<UserAtelierRead>> GetAllByGarmentId(int atelierId)
        {
            return await _userAtelierServiceQuery.GetTechniciansbyAtilierId(atelierId);
        }

    }
}
