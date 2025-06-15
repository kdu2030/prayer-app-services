using AutoMapper;
using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.PrayerRequests.Models;

namespace PrayerAppServices.PrayerRequests.Mappers {
    public class PrayerRequestModelMappingProfile : Profile {
        public PrayerRequestModelMappingProfile() {
            // TODO: Add support for IsUserPrayed
            CreateMap<PrayerRequest, PrayerRequestModel>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.RequestTitle, options => options.MapFrom(src => src.RequestTitle))
                .ForMember(dest => dest.RequestDescription, options => options.MapFrom(src => src.RequestDescription))
                .ForMember(dest => dest.CreatedDate, options => options.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.PrayerGroup, options => options.MapFrom(src => src.PrayerGroup))
                .ForMember(dest => dest.User, options => options.MapFrom(src => src.User))
                .ForMember(dest => dest.LikeCount, options => options.MapFrom(src => src.LikeCount))
                .ForMember(dest => dest.CommentCount, options => options.MapFrom(src => src.CommentCount))
                .ForMember(dest => dest.PrayedCount, options => options.MapFrom(src => src.PrayedCount))
                .ForMember(dest => dest.ExpirationDate, options => options.MapFrom(src => src.ExpirationDate))
                .ForMember(dest => dest.IsUserLiked, options => options.MapFrom((src, dest, destMember, context) => {
                    HashSet<int?> likedRequestIds = (HashSet<int?>)context.Items["LikedRequestIds"];
                    return likedRequestIds.Contains(src.Id);
                }))
                .ForMember(dest => dest.IsUserCommented, options => options.MapFrom((src, dest, destMember, context) => {
                    HashSet<int?> commentedRequestIds = (HashSet<int?>)context.Items["CommentedRequestIds"];
                    return commentedRequestIds.Contains(src.Id);
                }));
        }
    }
}
