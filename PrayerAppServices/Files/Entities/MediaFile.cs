using PrayerAppServices.Files.Constants;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.Users.Entities;

namespace PrayerAppServices.Files.Entities {
    public class MediaFile : MediaFileBase {
        public ICollection<PrayerGroup>? PrayerGroups { get; set; }

        public ICollection<AppUser>? AppUsers { get; set; }

        public static FileType GetFileTypeFromContentType(string contentType) {
            switch (contentType) {
                case ContentType.Jpg:
                case ContentType.Png:
                    return FileType.Image;
                default:
                    return FileType.Unknown;
            }
        }

    }
}
