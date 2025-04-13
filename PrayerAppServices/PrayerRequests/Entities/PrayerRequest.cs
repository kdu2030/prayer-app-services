using PrayerAppServices.PrayerGroups.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrayerAppServices.PrayerRequests.Entities {
    public class PrayerRequest {
        public int? Id { get; set; }

        [Column(TypeName = "varchar(255)")]
        public required string RequestTitle { get; set; }
        public required string RequestDescription { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required PrayerGroup PrayerGroup { get; set; }

        // TODO: Add additional fields, e.g. User that created it, expiration date, etc.

    }
}
