﻿using AutoMapper;
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
        }
    }
}
