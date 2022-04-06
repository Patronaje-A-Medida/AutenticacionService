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
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;
using Firebase.Storage;
using Firebase.Auth;
using AutenticacionService.Business.Utils;

namespace AutenticacionService.Business.ServicesCommand.Implements
{
    public class UserClientServiceCommand : IUserClientServiceCommand
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<UserBase> _userManager;
        private readonly SignInManager<UserBase> _signInManager;
        private readonly IConfigurationManager _configManager;

        public UserClientServiceCommand(
            IUnitOfWork uow, 
            IMapper mapper, 
            UserManager<UserBase> userManager, 
            SignInManager<UserBase> signInManager,
            IConfigurationManager configManager)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configManager = configManager;
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

        public async Task<UserClientRead> Update(UserClientUpdate userClientUpdate)
        {
            try
            {
                var userClient = (
                    await _uow.userClientRepository
                        .Get(filter: x => x.Id == userClientUpdate.Id, includeProperties: "User"))
                    .FirstOrDefault();

                if (userClient == null) return null;

                userClient.User.NameUser = userClientUpdate.NameUser;
                userClient.User.UserName = userClientUpdate.NameUser;
                userClient.User.LastNameUser = userClientUpdate.LastNameUser;
                userClient.User.NormalizedUserName = userClientUpdate.NameUser.ToUpper();
                userClient.Phone = userClientUpdate.Phone;
                userClient.Height = userClientUpdate.Height;
                userClient.Status = true;

                var userUpdated = _uow.userClientRepository.Update(userClient);

                if (userUpdated == null) return null;

                await _uow.SaveChangesAsync();
                var userRead = _mapper.Map<UserClientRead>(userUpdated);
                return userRead;
            }
            catch(RepositoryException ex)
            {
                throw new ServiceException(HttpStatusCode.InternalServerError, ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    HttpStatusCode.InternalServerError,
                    ErrorsCode.UPDATE_USER_FAILED,
                    ErrorMessages.UPDATE_USER_FAILED,
                    ex);
            }
        }

        public async Task<string> UploadImageProfile(int id, IFormFile imageFile)
        {
            try
            {
                if (imageFile.Length < 1) return null;

                var userClient = (
                        await _uow.userClientRepository
                            .Get(filter: x => x.Id == id))
                        .FirstOrDefault();

                if (userClient == null) return null;

                var urlImg = await UploadImageToFirebase(imageFile, $"client-{id}");
                userClient.ImageProfile = urlImg;
                var userUpdated = _uow.userClientRepository.Update(userClient);

                if (userUpdated == null) return null;

                await _uow.SaveChangesAsync();
                return urlImg;
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException(HttpStatusCode.InternalServerError, ex);
            }
            catch (ServiceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    HttpStatusCode.InternalServerError,
                    ErrorsCode.ADD_IMAGE_PROFILE,
                    ErrorMessages.ADD_IMAGE_PROFILE,
                    ex
                );
            }
        }

        private async Task<string> UploadImageToFirebase(IFormFile imageFile, string nameFile)
        {
            try
            {
                var storageOptions = await LogInFirebase();

                using (var memoryStream = new MemoryStream())
                {
                    imageFile.CopyTo(memoryStream);
                    var imageByteArrayToStream = ConvertByteArrayToStream(memoryStream.ToArray());
                    var imageStream = await imageByteArrayToStream.ReadAsStreamAsync();
                    var imgUrl = await UploadToFirebase(
                                storageOptions,
                                imageStream,
                                nameFile,
                                "profiles"
                    );
                    return imgUrl;
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    HttpStatusCode.InternalServerError,
                    ErrorsCode.ADD_IMAGE_PROFILE,
                    ErrorMessages.ADD_IMAGE_PROFILE,
                    ex
                );
            }
        }

        private StreamContent ConvertByteArrayToStream(byte[] imageByteArray)
        {
            StreamContent streamContent = new StreamContent(new MemoryStream(imageByteArray));
            return streamContent;
        }

        private async Task<string> UploadToFirebase(
            FirebaseStorageOptions storageOptions,
            Stream imageStream,
            string fileName,
            string folderPath
            )
        {
            var storageTask = new FirebaseStorage(_configManager.FirebaseBucket, storageOptions)
                .Child(folderPath)
                .Child(fileName)
                .PutAsync(imageStream);

            var url = await storageTask;
            return url;
        }

        private async Task<FirebaseStorageOptions> LogInFirebase()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(_configManager.FirebaseApiKey));
            var authConfig = await authProvider.SignInWithEmailAndPasswordAsync(_configManager.FirebaseAuthEmail, _configManager.FirebaseAuthPwd);

            var firebaseStorageOptions = new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(authConfig.FirebaseToken),
                ThrowOnCancel = true,
            };

            return firebaseStorageOptions;
        }
    }
}
