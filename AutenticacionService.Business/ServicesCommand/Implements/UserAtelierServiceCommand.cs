using AutenticacionService.Business.ServicesCommand.Interfaces;
using AutenticacionService.Domain.Base;
using AutenticacionService.Domain.Entities;
using AutenticacionService.Domain.Models;
using AutenticacionService.Persistence.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

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

        public async Task<UserAtelierRead> Create(UserOwnerCreate userOwnerCreate)
        {
            var userBase = _mapper.Map<UserBase>(userOwnerCreate);
            string userPassword = userOwnerCreate.Password;
            var createdUser = await _userManager.CreateAsync(userBase, userPassword);

            if (!createdUser.Succeeded) throw new Exception("error userbase");

            var user = await _userManager.FindByEmailAsync(userBase.Email);
            var userAtelier = _mapper.Map<UserAtelier>(userOwnerCreate);
            userAtelier.UserId = user.Id;
            var createdUserAtelier = await _uow.userAtelierRepository.Add(userAtelier);

            if (createdUserAtelier == null) throw new Exception("error useratelier");

            await _uow.SaveChangesAsync();
            var userAtelierRead = _mapper.Map<UserAtelierRead>(createdUserAtelier);
            return userAtelierRead;
        }
    }
}
