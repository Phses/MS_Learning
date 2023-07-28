using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformProfiles: Profile
    {
        public PlatformProfiles()
        {
            // Source ----> target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();
            CreateMap<Platform, GrpcPlatformModel>().ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
