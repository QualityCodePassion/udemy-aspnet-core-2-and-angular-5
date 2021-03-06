using System;
using System.Linq;
using AutoMapper;
using SocialApp.API.Dtos;
using SocialApp.API.Models;

namespace SocialApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, options => 
                    options.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMainPhoto).Url))
                .ForMember(dest => dest.Age, options => 
                    options.MapFrom(d => d.DateOfBirth.CalculateAge()));

            CreateMap<User, UserForDetailed>()
                .ForMember(dest => dest.PhotoUrl, options => 
                    options.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMainPhoto).Url))
                .ForMember(dest => dest.Age, options => 
                    options.MapFrom(d => d.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotosForDetailedDto>();

            CreateMap<UserForUpdateDto, User>();
        }
    }
}