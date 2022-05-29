using AutenticacionService.Business.Handlers;
using AutenticacionService.Business.ServicesCommand.Interfaces;
using AutenticacionService.Business.ServiceSmtp;
using AutenticacionService.Business.Utils;
using AutenticacionService.Domain.Base;
using AutenticacionService.Domain.Entities;
using AutenticacionService.Domain.Models;
using AutenticacionService.Persistence.Handlers;
using AutenticacionService.Persistence.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Security.Cryptography;
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
        private readonly IEmailSender _emailSender;

        public UserAtelierServiceCommand(
            IUnitOfWork uow, 
            IMapper mapper, 
            UserManager<UserBase> userManager, 
            SignInManager<UserBase> signInManager,
            IEmailSender emailSender)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<UserAtelierRead> CreateOwner(UserOwnerCreate userOwnerCreate)
        {
            var userBase = _mapper.Map<UserBase>(userOwnerCreate);
            var userAtelier = _mapper.Map<UserAtelier>(userOwnerCreate);
            var result = await Create(userBase, userAtelier, userOwnerCreate.Password);
            if (result != null)
            {
                var message = BuildMessageCreateOwner(userOwnerCreate.Email, userOwnerCreate.NameUser);
                await _emailSender.SendEmail(message);
            }
            return result;
        }

        public async Task<UserAtelierRead> CreateTechnician(UserTechnicianCreate userTechnicianCreate)
        {
            var userBase = _mapper.Map<UserBase>(userTechnicianCreate);
            var userAtelier = _mapper.Map<UserAtelier>(userTechnicianCreate);
            var pwd = GenerateToken(10);
            var result = await Create(userBase, userAtelier, pwd);
            if (result != null)
            {
                var message = BuildMessageCreateTechnician(userTechnicianCreate.Email, userTechnicianCreate.NameUser, pwd);
                await _emailSender.SendEmail(message);
            }
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

        private Message BuildMessageCreateTechnician(string email, string userName, string password)
        {
            string subject = "Creación de cuenta personal";

            string content = $"<html>" +
                $"<head>" +
                $"<title></title>" +
                $"</head>" +
                $"<body>" +
                $"<table align='center' border='0' cellpadding='0' cellspacing='0' class='deviceWidth' style='width:100%;min-width:100%' width='100%'>" +
                $"<tbody>" +
                $"<tr>" +
                $"<td align='center' bgcolor='#ffffff' vertical-align:='top'>" +
                $"<table border='0' cellpadding='10' cellspacing='0' class='deviceWidth' style='width:100%;max-width:700px;' width='700'>" +
                $"<tbody style='color:rgb(74,74,74);font-family:Open Sans,Helvetica Neue,Helvetica,Arial,sans-serif;font-size:15px'>" +
                $"<tr>" +
                $"<td>" +
                $"<table align='center' border='0' cellpadding='0' cellspacing='0' class='deviceWidth'>" +
                $"<tbody>" +
                $"<tr>" +
                $"<td><a id='ext-gen1933'><img alt='' src='https://images.email-platform.com/fundacionvsl/PATRONAJE.jpeg' style='opacity: 0.9; width: 480px; max-width: 700px; border-width: 0px; border-style: solid; margin: 0px; height: 399px;' /></a></td>" +
                $"</tr>" +
                $"</tbody>" +
                $"</table>" +
                $"<p style='text-align: center;'><strong><span style='font-size:22px;'>&iexcl;Hola {userName}, bienvenido/a&nbsp;a Patronaje App!</span></strong></p>" +
                $"<p style='text-align: center;'><span style='font-size:18px;'>Su cuenta para el ingreso a la aplicaci&oacute;n ha sido creada. Puede acceder a la aplicaci&oacute;n con las siguientes credenciales:</span></p>" +
                $"<p style='text-align: center;'><span style='font-size:18px;'><strong><span style='color:#e67e22;'>Correo: {email}</span></strong></span></p>" +
                $"<p style='text-align: center;'><span style='font-size:18px;'><strong><span style='color:#e67e22;'>Contrase&ntilde;a: {password}</span></strong></span></p>" +
                $"<p style='text-align: center;'>&nbsp;</p>" +
                $"<p>&nbsp;</p>" +
                $"</td>" +
                $"</tr>" +
                $"</tbody>" +
                $"</table>" +
                $"</td>" +
                $"</tr>" +
                $"</tbody>" +
                $"</table>" +
                $"</body>" +
                $"</html>";

            var msg = new Message(to: email, subject: subject, content: content);
            return msg;
        }

        private Message BuildMessageCreateOwner(string email, string userName)
        {
            string subject = "Confirmación de registro de usuario";

            string content = $"<html>" +
                $"<head>" +
                $"<title></title>" +
                $"</head>" +
                $"<body>" +
                $"<table align='center' border='0' cellpadding='0' cellspacing='0' class='deviceWidth' style='width:100%;min-width:100%' width='100%'>" +
                $"<tbody>" +
                $"<tr>" +
                $"<td align='center' bgcolor='#ffffff' vertical-align:='top'>" +
                $"<table border='0' cellpadding='10' cellspacing='0' class='deviceWidth' style='width:100%;max-width:700px;' width='700'>" +
                $"<tbody style='color:rgb(74,74,74);font-family:Open Sans,Helvetica Neue,Helvetica,Arial,sans-serif;font-size:15px'>" +
                $"<tr>" +
                $"<td>" +
                $"<table align='center' border='0' cellpadding='0' cellspacing='0' class='deviceWidth'>" +
                $"<tbody>" +
                $"<tr>" +
                $"<td><a id='ext-gen1933'><img alt='' src='https://images.email-platform.com/fundacionvsl/PATRONAJE.jpeg' style='opacity: 0.9; width: 480px; max-width: 700px; border-width: 0px; border-style: solid; margin: 0px; height: 399px;' /></a></td>" +
                $"</tr>" +
                $"</tbody>" +
                $"</table>" +
                $"<p style='text-align: center;'><strong><span style='font-size:22px;'>&iexcl;Hola {userName}, bienvenido/a&nbsp;a Patronaje App!</span></strong></p>" +
                $"<p style='text-align: center;'><span style='font-size:18px;'>Su cuenta para el ingreso a la aplicaci&oacute;n ha sido creada. ¡Ya puedes gestionar tus pedidos de prendas y generar los patrones a la medida de tus clientes!</span></p>" +
                $"<p style='text-align: center;'>&nbsp;</p>" +
                $"<p>&nbsp;</p>" +
                $"</td>" +
                $"</tr>" +
                $"</tbody>" +
                $"</table>" +
                $"</td>" +
                $"</tr>" +
                $"</tbody>" +
                $"</table>" +
                $"</body>" +
                $"</html>";


            var msg = new Message(to: email, subject: subject, content: content);
            return msg;
        }

        private string GenerateToken(int length)
        {
            using (RNGCryptoServiceProvider cryptRNG = new RNGCryptoServiceProvider())
            {
                byte[] tokenBuffer = new byte[length];
                cryptRNG.GetBytes(tokenBuffer);
                return Convert.ToBase64String(tokenBuffer);
            }
        }

    }
}
