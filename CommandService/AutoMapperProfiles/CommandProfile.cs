using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;
using PlatformService;

namespace CommandService.AutoMapperProfiles
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();

            CreateMap<PlatformPublishDto, Platform>()
                .ForMember(destinationMember =>
                    destinationMember.ExternalId,
                    opt => opt.MapFrom(src => src.Id));

            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(destination =>
                        destination.ExternalId,
                    opt => opt.MapFrom(src => src.PlatformId))
                .ForMember(destination =>
                        destination.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(destination => destination.Commands, opt => opt.Ignore());
        }
    }
}
