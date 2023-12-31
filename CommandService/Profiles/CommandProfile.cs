﻿using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.Profiles
{
    public class CommandProfile: Profile
    {
        public CommandProfile()
        {
            //source --> target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<PlatformPublishedDto, Platform>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcPlatformModel, Platform>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId));
        }
    }
}