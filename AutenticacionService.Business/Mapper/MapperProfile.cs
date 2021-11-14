using AutenticacionService.Domain.Base;
using AutenticacionService.Domain.Entities;
using AutenticacionService.Domain.Models;
using AutoMapper;

namespace AutenticacionService.Business.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            /*CreateMap<UserClientCreate, UserClient>()
                .ForMember()*/
            CreateMap<UserClientCreate, UserBase>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.NameUser)) // IdentityUser
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)) // IdentityUser
                .ForMember(dest => dest.NameUser, opt => opt.MapFrom(src => src.NameUser)) // UserBase
                .ForMember(dest => dest.LastNameUser, opt => opt.MapFrom(src => src.LastNameUser)) // UserBase
                .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(src => false)) // UserBase
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "CLIENTE")) // UserBase
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "ACTIVO")); // UserBase

            CreateMap<UserClientCreate, UserClient>()
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone));

            CreateMap<UserClient, UserClientRead>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.NameUser, opt => opt.MapFrom(src => src.User.NameUser))
                .ForMember(dest => dest.LastNameUser, opt => opt.MapFrom(src => src.User.LastNameUser))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.User.Role));
        }
    }
}
