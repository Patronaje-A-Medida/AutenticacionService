using AutenticacionService.Domain.Base;
using AutenticacionService.Domain.Entities;
using AutenticacionService.Domain.Models;
using AutenticacionService.Domain.Utils;
using AutoMapper;

namespace AutenticacionService.Business.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            /*
             * Utilizar el ForMember para el mapeo de atributos que se crean necesarios especificar,
             * ya sea porque el atributo pertenece a un objeto anidado 
             * o el nombre del atributo source es diferente al del target.
             */
            CreateMap<UserClientCreate, UserBase>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.NameUser)) // IdentityUser
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)) // IdentityUser
                .ForMember(dest => dest.NameUser, opt => opt.MapFrom(src => src.NameUser)) // UserBase
                .ForMember(dest => dest.LastNameUser, opt => opt.MapFrom(src => src.LastNameUser)) // UserBase
                .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(src => false)) // UserBase
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => RolesUtil.CLIENT)) // UserBase
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StatusUtil.USER_ACTIVE)); // UserBase

            CreateMap<UserClientCreate, UserClient>();

            CreateMap<UserClient, UserClientRead>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.NameUser, opt => opt.MapFrom(src => src.User.NameUser))
                .ForMember(dest => dest.LastNameUser, opt => opt.MapFrom(src => src.User.LastNameUser))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.User.Role));

            CreateMap<UserOwnerCreate, UserBase>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.NameUser)) // IdentityUser
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)) // IdentityUser
                .ForMember(dest => dest.NameUser, opt => opt.MapFrom(src => src.NameUser)) // UserBase
                .ForMember(dest => dest.LastNameUser, opt => opt.MapFrom(src => src.LastNameUser)) // UserBase
                .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(src => false)) // UserBase
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => RolesUtil.OWNER)) // UserBase
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StatusUtil.USER_ACTIVE)); // UserBase

            CreateMap<UserOwnerCreate, UserAtelier>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => RolesUtil.OWNER))
                .ForMember(dest => dest.Atelier, opt => opt.MapFrom(src => src.Atelier));

            CreateMap<AtelierCreate, Atelier>();

            CreateMap<UserAtelier, UserAtelierRead>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.NameUser, opt => opt.MapFrom(src => src.User.NameUser))
                .ForMember(dest => dest.LastNameUser, opt => opt.MapFrom(src => src.User.LastNameUser))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.User.Role))
                .ForMember(dest => dest.AtelierId, opt => opt.MapFrom(src => src.Atelier != null ? src.Atelier.Id : src.AtelierId));

            CreateMap<UserTechnicianCreate, UserBase>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.NameUser)) // IdentityUser
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)) // IdentityUser
                .ForMember(dest => dest.NameUser, opt => opt.MapFrom(src => src.NameUser)) // UserBase
                .ForMember(dest => dest.LastNameUser, opt => opt.MapFrom(src => src.LastNameUser)) // UserBase
                .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(src => false)) // UserBase
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => RolesUtil.TECHNICIAN)) // UserBase
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StatusUtil.USER_ACTIVE)); // UserBase

            CreateMap<UserTechnicianCreate, UserAtelier>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => RolesUtil.TECHNICIAN));

        }
    }
}
