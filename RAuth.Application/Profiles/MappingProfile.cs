using AutoMapper;
using RAuth.Application.DTO.AuthDTO;
using RAuth.Application.DTO.UserDTO;
using RAuth.Core.Models.AddressModel;
using RAuth.Core.Models.User;

namespace RAuth.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserDTO, ApplicationUser>().ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<UpdateAddressDTO, Address>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
