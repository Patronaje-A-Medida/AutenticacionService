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
    public class UserAtelierServiceQuery : IUserAtelierServiceQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<UserBase> _userManager;
        private readonly SignInManager<UserBase> _signInManager;

        public UserAtelierServiceQuery(
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

        public async Task<UserAtelierRead> SignIn(UserLogin userLogin)
        {
            try
            {
                var userBase = await _userManager.FindByEmailAsync(userLogin.Email);

                if (userBase == null) return null;

                if (!userBase.Role.Equals(RolesUtil.OWNER) &&
                    !userBase.Role.Equals(RolesUtil.TECHNICIAN))
                    throw new Exception("error service wrong role");

                var checkPassword = await _userManager.CheckPasswordAsync(userBase, userLogin.Password);

                if (!checkPassword) return null;

                var signInResult = await _signInManager.PasswordSignInAsync(
                      userBase.UserName,
                      userLogin.Password,
                      isPersistent: false,
                      lockoutOnFailure: true);

                if (!signInResult.Succeeded || signInResult.IsLockedOut) return null;

                // TODO agregar logica de bloqueo

                var userAtelier = await _uow.userAtelierRepository.GetByUserId_Role(userBase.Id, userBase.Role);
                var userAtelierRead = _mapper.Map<UserAtelierRead>(userAtelier);
                return userAtelierRead;
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
