using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;

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
        }
    }
}
