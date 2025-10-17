using AutoMapper;
using PrayerAppServices.Files.Constants;
using PrayerAppServices.Files.Entities;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups.Mappers
{
    public class PrayerGroupMappingProfile : Profile
    {
        public PrayerGroupMappingProfile()
        {
            CreateMap<PrayerGroupRequest, PrayerGroup>()
                .ForMember(dest => dest.PrayerGroupId, options => options.MapFrom((src, dest, destMember, context) => context.Items["Id"]))
                .ForMember(dest => dest.AvatarFile, options => options.MapFrom((src, dest, destMember, context) => context.Items["ImageFile"]))
                .ForMember(dest => dest.AvatarFileId, options => options.MapFrom(src => src.AvatarFileId))
                .ForMember(dest => dest.BannerFile, options => options.MapFrom((src, dest, destMember, context) => context.Items["BannerImageFile"]))
                .ForMember(dest => dest.BannerFileId, options => options.MapFrom(src => src.BannerFileId))
                .ForMember(dest => dest.GroupName, options => options.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.Description, options => options.MapFrom(src => src.Description))
                .ForMember(dest => dest.Rules, options => options.MapFrom(src => src.Rules))
                .ForMember(dest => dest.VisibilityLevel, options => options.MapFrom(src => src.VisibilityLevel));


            CreateMap<PrayerGroup, PrayerGroupDetails>()
                .ForMember(dest => dest.PrayerGroupId, options => options.MapFrom(src => src.PrayerGroupId))
                .ForMember(dest => dest.GroupName, options => options.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.Description, options => options.MapFrom(src => src.Description))
                .ForMember(dest => dest.Rules, options => options.MapFrom(src => src.Rules))
                .ForMember(dest => dest.AvatarFile, options => options.MapFrom(src => src.AvatarFile))
                .ForMember(dest => dest.BannerFile, options => options.MapFrom(src => src.BannerFile))
                .ForMember(dest => dest.JoinStatus, options => options.MapFrom((src, dest, destMember, context) => context.Items.GetValueOrDefault("JoinStatus")))
                .ForMember(dest => dest.Admins, options => options.MapFrom((src, dest, destMember, context) => context.Items.GetValueOrDefault("Admins")))
                .ForMember(dest => dest.UserRole, options => options.MapFrom((src, dest, destMember, context) => context.Items.GetValueOrDefault("UserRole")))
                .ForMember(dest => dest.VisibilityLevel, options => options.MapFrom(src => src.VisibilityLevel));

            CreateMap<PrayerGroup, PrayerGroupDetails>()
                .ForMember(dest => dest.PrayerGroupId, options => options.MapFrom(src => src.PrayerGroupId))
                .ForMember(dest => dest.GroupName, options => options.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.Description, options => options.MapFrom(src => src.Description))
                .ForMember(dest => dest.Rules, options => options.MapFrom(src => src.Rules))
                .ForMember(dest => dest.VisibilityLevel, options => options.MapFrom(src => src.VisibilityLevel))
                .ForMember(dest => dest.AvatarFile, options => options.MapFrom(src => src.AvatarFile));

            CreateMap<PrayerGroupUserEntity, PrayerGroupUserSummary>()
                .ForMember(dest => dest.UserId, options => options.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FullName, options => options.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Username, options => options.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PrayerGroupRole, options => options.MapFrom(src => src.GroupRole))
                .ForMember(dest => dest.Image, options => options.MapFrom(src => src.ImageFileId != null ? new MediaFile { MediaFileId = src.ImageFileId, FileName = src.FileName ?? "", FileUrl = src.FileUrl ?? "", FileType = FileType.Image } : null));

            CreateMap<PrayerGroupSummaryEntity, PrayerGroupDetails>()
                .ForMember(dest => dest.PrayerGroupId, options => options.MapFrom(src => src.PrayerGroupId))
                .ForMember(dest => dest.GroupName, options => options.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.AvatarFile, options => options.MapFrom(src => src.MediaFileId != null ? new MediaFile { MediaFileId = src.MediaFileId, FileName = src.FileName ?? "", FileUrl = src.FileUrl ?? "", FileType = src.FileType ?? FileType.Image } : null));

            CreateMap<PrayerGroupAppUser, PrayerGroupUserToAdd>()
                .ForMember(dest => dest.UserId, options => options.MapFrom(src => src.UserId))
                .ForMember(dest => dest.PrayerGroupRole, options =>
                    options.MapFrom(src => src.PrayerGroupRole.HasValue ? (int)src.PrayerGroupRole : (int)PrayerGroupRole.Member));


        }

    }
}
