using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // map from, map to
            CreateMap<AppUser, MemberDto>()
                // in order to populate PhotoUrl in MemberDto
                // ForMember means which property do we want to refer
                // destination , source
                // looping through all the photos and search for a photo which IsMain is true then return the Url of that photo
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();

            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
        }
    }
}
