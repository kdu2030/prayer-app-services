using AutoMapper;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups.Mappers {
    public class PrayerGroupMappingProfile : Profile {
        public PrayerGroupMappingProfile() {
            CreateMap<PrayerGroupRequest, PrayerGroup>()
                .ForMember(dest => dest.Id, options => options.MapFrom((src, dest, destMember, context) => context.Items["Id"]))
                .ForMember(dest => dest.ImageFile, options => options.MapFrom((src, dest, destMember, context) => context.Items["ImageFile"]))
                .ForMember(dest => dest.GroupName, options => options.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.Description, options => options.MapFrom(src => src.Description))
                .ForMember(dest => dest.Rules, options => options.MapFrom(src => src.Rules))
                .ForMember(dest => dest.Color, options => options.MapFrom(src => src.Color));

        }

    }
}
