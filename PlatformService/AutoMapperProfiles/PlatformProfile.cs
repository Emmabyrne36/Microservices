using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.AutoMapperProfiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishDto>();
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(destination => destination.PlatformId, opt => opt.MapFrom(src => src.Id))
                .ForMember(destination => destination.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(destination => destination.Publisher, opt => opt.MapFrom(src => src.Publisher));

            CreateMap<IEnumerable<Platform>, PlatformResponse>();
        }
    }
}
