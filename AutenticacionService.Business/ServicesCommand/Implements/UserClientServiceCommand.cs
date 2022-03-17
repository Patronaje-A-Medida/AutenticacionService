using AutenticacionService.Business.Handlers;
using AutenticacionService.Business.ServicesCommand.Interfaces;
using AutenticacionService.Domain.Base;
using AutenticacionService.Domain.Entities;
using AutenticacionService.Domain.Models;
using AutenticacionService.Persistence.UnitOfWork;
using AutenticacionService.Persistence.Handlers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading.Tasks;
using System;
using static AutenticacionService.Domain.Utils.ErrorsUtil;

namespace AutenticacionService.Business.ServicesCommand.Implements
{
    public class UserClientServiceCommand : IUserClientServiceCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<UserBase> _userManager;
        private readonly SignInManager<UserBase> _signInManager;

        public UserClientServiceCommand(
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

        public async Task<UserClientRead> Create(UserClientCreate userClientCreate)
        {
            try
            {
                var userBase = _mapper.Map<UserBase>(userClientCreate);
                string userPassword = userClientCreate.Password;
                var createdUser = await _userManager.CreateAsync(userBase, userPassword);

                if (!createdUser.Succeeded) return null;

                var user = await _userManager.FindByEmailAsync(userBase.Email);
                var userClient = _mapper.Map<UserClient>(userClientCreate);
                userClient.UserId = user.Id;
                var createdUserClient = await _uow.userClientRepository.Add(userClient);

                if (createdUserClient == null) return null;

                await _uow.SaveChangesAsync();
                var userClientRead = _mapper.Map<UserClientRead>(createdUserClient);
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
                    ErrorsCode.SIGN_UP_ERROR, 
                    ErrorMessages.SIGN_UP_ERROR,
                    ex);
            }
        }
    }
}
