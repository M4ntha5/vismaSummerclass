using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WordEntity, Anagram>()
                .ForMember(dest =>
                    dest.Case,
                    opt => opt.MapFrom(src => src.Category))
                .ForMember(dest =>
                    dest.Word,
                    opt => opt.MapFrom(src => src.Word))
                .ForMember(dest =>
                    dest.ID,
                    opt => opt.MapFrom(src => src.ID))
                .ReverseMap();

            CreateMap<UserLogEntity, UserLog>()
                .ForMember(dest =>
                    dest.SearchTime,
                    opt => opt.MapFrom(src => src.SearchTime))
                .ForMember(dest =>
                    dest.SearchPhrase,
                    opt => opt.MapFrom(src => src.Phrase))
                .ForMember(dest =>
                    dest.Ip,
                    opt => opt.MapFrom(src => src.Ip))
                .ForMember(dest =>
                    dest.Action,
                    opt => opt.MapFrom(src => src.Action))
                .ReverseMap();

            CreateMap<CachedWordEntity, CachedWord>()
                .ForMember(dest =>
                    dest.AnagramsIds,
                    opt => opt.MapFrom(src => src.AnagramsIds))
                .ForMember(dest =>
                    dest.SearchPhrase,
                    opt => opt.MapFrom(src => src.Phrase))
                .ReverseMap();
        }
    }
}
