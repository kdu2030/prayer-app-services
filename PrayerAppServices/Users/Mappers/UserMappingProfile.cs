using AutoMapper;
using PrayerAppServices.Users.Entities;
using PrayerAppServices.Users.Models;

namespace PrayerAppServices.Users.Mappers {
    public class UserMappingProfile : Profile {
        public UserMappingProfile() {
            CreateMap<AppUser, UserSummary>()
                .ForMember(dest => dest.UserId, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, options => options.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Image, options => options.MapFrom(src => src.Image))
                .ForMember(dest => dest.Username, options => options.MapFrom(src => src.UserName));
        }

    }
}
