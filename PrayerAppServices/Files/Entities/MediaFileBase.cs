using PrayerAppServices.Files.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrayerAppServices.Files.Entities {
    public class MediaFileBase {
        public int? Id { get; set; }

        [Column(TypeName = "varchar(255)")]
        public required string FileName { get; set; }

        [Column(TypeName = "varchar(255)")]
        public required string Url { get; set; }
        public required FileType FileType { get; set; }
    }
}
