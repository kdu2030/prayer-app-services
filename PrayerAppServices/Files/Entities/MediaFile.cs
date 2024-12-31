using PrayerAppServices.Files.Constants;

namespace PrayerAppServices.Files.Entities {
    public class MediaFile : MediaFileBase {
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
