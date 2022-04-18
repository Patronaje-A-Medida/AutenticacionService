using AutenticacionService.Business.Handlers;
using AutenticacionService.Business.ServicesCommand.Interfaces;
using AutenticacionService.Domain.Base;
using AutenticacionService.Domain.Entities;
using AutenticacionService.Domain.Models;
using AutenticacionService.Persistence.Handlers;
using AutenticacionService.Persistence.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Threading.Tasks;
using static AutenticacionService.Domain.Utils.ErrorsUtil;

namespace AutenticacionService.Business.ServicesCommand.Implements
{
    public class UserAtelierServiceCommand : IUserAtelierServiceCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<UserBase> _userManager;
        private readonly SignInManager<UserBase> _signInManager;

        public UserAtelierServiceCommand(
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

        public async Task<UserAtelierRead> CreateOwner(UserOwnerCreate userOwnerCreate)
        {
            var userBase = _mapper.Map<UserBase>(userOwnerCreate);
            var userAtelier = _mapper.Map<UserAtelier>(userOwnerCreate);
            var result = await Create(userBase, userAtelier, userOwnerCreate.Password);
            return result;
        }

        public async Task<UserAtelierRead> CreateTechnician(UserTechnicianCreate userTechnicianCreate)
        {
            var userBase = _mapper.Map<UserBase>(userTechnicianCreate);
            var userAtelier = _mapper.Map<UserAtelier>(userTechnicianCreate);
            var result = await Create(userBase, userAtelier, userTechnicianCreate.Password);
            return result;
        }

        private async Task<UserAtelierRead> Create(UserBase userBase, UserAtelier userAtelier, string password)
        {
            try
            {
                var createdUser = await _userManager.CreateAsync(userBase, password);

                if (!createdUser.Succeeded) return null;

                var user = await _userManager.FindByEmailAsync(userBase.Email);
                userAtelier.UserId = user.Id;
                var createdUserAtelier = await _uow.userAtelierRepository.Add(userAtelier);

                if (createdUserAtelier == null) return null;

                await _uow.SaveChangesAsync();
                var userAtelierRead = _mapper.Map<UserAtelierRead>(createdUserAtelier);
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
                    ErrorsCode.SIGN_UP_ERROR,
                    ErrorMessages.SIGN_UP_ERROR,
                    ex);
            }
        }

    }
}
