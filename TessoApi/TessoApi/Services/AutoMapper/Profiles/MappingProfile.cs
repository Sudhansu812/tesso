using AutoMapper;
using TessoApi.DTOs.Auth;
using TessoApi.DTOs.Project;
using TessoApi.Models;
using TessoApi.Models.Identity;

namespace TessoApi.Services.AutoMapper.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(
                    dest => dest.Email,
                    src => src.MapFrom(x => x.EmailAddress)
                )
                .ForMember(
                    dest => dest.UserName,
                    src => src.MapFrom(x => x.UserName)
                )
                .ForMember(
                    dest => dest.FirstName,
                    src => src.MapFrom(x => x.FirstName)
                )
                .ForMember(
                    dest => dest.LastName,
                    src => src.MapFrom(x => x.LastName)
                )
                .ReverseMap();
        }
    }
}
