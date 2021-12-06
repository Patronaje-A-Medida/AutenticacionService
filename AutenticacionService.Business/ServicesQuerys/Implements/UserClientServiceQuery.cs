﻿using AutenticacionService.Business.ServicesQuerys.Interfaces;
using AutenticacionService.Domain.Base;
using AutenticacionService.Domain.Models;
using AutenticacionService.Domain.Utils;
using AutenticacionService.Persistence.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

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
            var userBase = await _userManager.FindByEmailAsync(userLogin.Email);

            if (!userBase.Role.Equals(RolesUtil.CLIENT)) throw new Exception("error service wrong role");

            var checkPassword = await _userManager.CheckPasswordAsync(userBase, userLogin.Password);

            if (!checkPassword) throw new Exception("error service wrong password");

            var signInResult = await _signInManager.PasswordSignInAsync(
                  userBase.UserName,
                  userLogin.Password,
                  isPersistent: false,
                  lockoutOnFailure: true);

            if (!signInResult.Succeeded) throw new Exception("error service sign in");

            if (signInResult.IsLockedOut) throw new Exception("error service user blocked"); // TODO agregar logica de bloqueo

            var userClient = await _uow.userClientRepository.GetByUserId(userBase.Id);
            var userClientRead = _mapper.Map<UserClientRead>(userClient);
            return userClientRead;
        }
    }
}
