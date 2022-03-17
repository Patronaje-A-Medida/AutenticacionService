using AutenticacionService.Business.Handlers;
using AutenticacionService.Business.ServicesQuerys.Interfaces;
using AutenticacionService.Domain.Base;
using AutenticacionService.Domain.Models;
using AutenticacionService.Domain.Utils;
using AutenticacionService.Persistence.Handlers;
using AutenticacionService.Persistence.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Threading.Tasks;
using static AutenticacionService.Domain.Utils.ErrorsUtil;

namespace AutenticacionService.Business.ServicesQuerys.Implements
{
    public class UserClientServiceQuery : IUserClientServiceQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<UserBase> _userManager;
        private readonly SignInManager<UserBase> _signInManager;

        public UserClientServiceQuery(
            IUnitOfWork uow, 
            IMapper mapper, 
            UserManager<UserBase> userManager, 
            SignInManager<UserBase> signInManager)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<UserClientRead> SignIn(UserLogin userLogin)
        {
            try
            {
                var userBase = await _userManager.FindByEmailAsync(userLogin.Email);

                if (!userBase.Role.Equals(RolesUtil.CLIENT)) return null;

                var checkPassword = await _userManager.CheckPasswordAsync(userBase, userLogin.Password);

                if (!checkPassword) return null;

                var signInResult = await _signInManager.PasswordSignInAsync(
                      userBase.UserName,
                      userLogin.Password,
                      isPersistent: false,
                      lockoutOnFailure: true);

                if (!signInResult.Succeeded || signInResult.IsLockedOut) return null;

                // TODO agregar logica de bloqueo

                var userClient = await _uow.userClientRepository.GetByUserId(userBase.Id);
                var userClientRead = _mapper.Map<UserClientRead>(userClient);
                return userClientRead;
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException(HttpStatusCode.InternalServerError, ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    HttpStatusCode.InternalServerError,
                    ErrorsCode.LOGIN_USER_ERROR,
                    ErrorMessages.LOGIN_USER_ERROR,
                    ex);
            }
        }
    }
}
