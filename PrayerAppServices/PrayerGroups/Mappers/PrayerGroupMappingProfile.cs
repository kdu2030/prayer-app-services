using AutoMapper;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.Utils;

namespace PrayerAppServices.PrayerGroups.Mappers {
    public class PrayerGroupMappingProfile : Profile {
        public PrayerGroupMappingProfile() {
            CreateMap<PrayerGroupRequest, PrayerGroup>()
                .ForMember(dest => dest.Id, options => options.MapFrom((src, dest, destMember, context) => context.Items["Id"]))
                .ForMember(dest => dest.ImageFile, options => options.MapFrom((src, dest, destMember, context) => context.Items["ImageFile"]))
                .ForMember(dest => dest.GroupName, options => options.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.Description, options => options.MapFrom(src => src.Description))
                .ForMember(dest => dest.Rules, options => options.MapFrom(src => src.Rules))
                .ForMember(dest => dest.Color, options => options.MapFrom(src => src.Color != null ? (int?)ColorUtils.ColorHexStringToInt(src.Color) : null));

            CreateMap<PrayerGroup, PrayerGroupDetails>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.GroupName, options => options.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.Description, options => options.MapFrom(src => src.Description))
                .ForMember(dest => dest.Rules, options => options.MapFrom(src => src.Rules))
                .ForMember(dest => dest.Color, options => options.MapFrom(src => src.Color != null ? ColorUtils.ColorIntToHexString(src.Color.Value) : null))
                .ForMember(dest => dest.ImageFile, options => options.MapFrom(src => src.ImageFile))
                .ForMember(dest => dest.IsUserJoined, options => options.MapFrom((src, dest, destMember, context) => context.Items.GetValueOrDefault("IsUserJoined")))
                .ForMember(dest => dest.Admins, options => options.MapFrom((src, dest, destMember, context) => context.Items.GetValueOrDefault("Admins")))
                .ForMember(dest => dest.UserRole, options => options.MapFrom((src, dest, destMember, context) => context.Items.GetValueOrDefault("UserRole")));

            CreateMap<PrayerGroup, PrayerGroupDetails>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.GroupName, options => options.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.Description, options => options.MapFrom(src => src.Description))
                .ForMember(dest => dest.Rules, options => options.MapFrom(src => src.Rules))
                .ForMember(dest => dest.Color, options => options.MapFrom(src => src.Color != null ? ColorUtils.ColorIntToHexString(src.Color.Value) : null))
                .ForMember(dest => dest.ImageFile, options => options.MapFrom(src => src.ImageFile));

        }

    }
}
