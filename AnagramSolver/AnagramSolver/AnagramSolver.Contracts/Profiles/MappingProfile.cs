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
        }
    }
}
