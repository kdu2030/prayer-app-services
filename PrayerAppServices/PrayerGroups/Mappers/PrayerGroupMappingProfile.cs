﻿using AutoMapper;
using PrayerAppServices.Files.Constants;
using PrayerAppServices.Files.Entities;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.Utils;

namespace PrayerAppServices.PrayerGroups.Mappers {
    public class PrayerGroupMappingProfile : Profile {
        public PrayerGroupMappingProfile() {
            CreateMap<PrayerGroupRequest, PrayerGroup>()
                .ForMember(dest => dest.Id, options => options.MapFrom((src, dest, destMember, context) => context.Items["Id"]))
                .ForMember(dest => dest.ImageFile, options => options.MapFrom((src, dest, destMember, context) => context.Items["ImageFile"]))
                .ForMember(dest => dest.ImageFileId, options => options.MapFrom(src => src.ImageFileId))
                .ForMember(dest => dest.BannerImageFile, options => options.MapFrom((src, dest, destMember, context) => context.Items["BannerImageFile"]))
                .ForMember(dest => dest.BannerImageFileId, options => options.MapFrom(src => src.BannerImageFileId))
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
                .ForMember(dest => dest.BannerImageFile, options => options.MapFrom(src => src.BannerImageFile))
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

            CreateMap<PrayerGroupUserEntity, PrayerGroupUserSummary>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, options => options.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Username, options => options.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Role, options => options.MapFrom(src => src.GroupRole))
                .ForMember(dest => dest.Image, options => options.MapFrom(src => src.ImageFileId != null ? new MediaFile { Id = src.ImageFileId, FileName = src.FileName ?? "", Url = src.FileUrl ?? "", FileType = FileType.Image } : null));

            CreateMap<PrayerGroupSummaryEntity, PrayerGroupDetails>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.GroupName, options => options.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.ImageFile, options => options.MapFrom(src => src.ImageFileId != null ? new MediaFile { Id = src.ImageFileId, FileName = src.FileName ?? "", Url = src.Url ?? "", FileType = src.FileType ?? FileType.Image } : null));

            CreateMap<PrayerGroupAppUser, PrayerGroupUserToAdd>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.PrayerGroupRole, options =>
                    options.MapFrom(src => src.PrayerGroupRole.HasValue ? (int)src.PrayerGroupRole : (int)PrayerGroupRole.Member));


        }

    }
}
