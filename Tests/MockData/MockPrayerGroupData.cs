

using PrayerAppServices.Files.Constants;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Entities;

namespace Tests.MockData {
    public static class MockPrayerGroupData {
        public static readonly IEnumerable<PrayerGroupAdminUser> prayerGroupAdminUsers = [
            new PrayerGroupAdminUser {
                Id = 1,
                FullName = "Anakin Skywalker",
                GroupRole = PrayerGroupRole.Admin,
                ImageFileId = 1,
                FileName = "anakin_skywalker.png",
                FileType = FileType.Image,
            },
            new PrayerGroupAdminUser {
                Id = 2,
                FullName = "Obi Wan Kenobi"
            },
            new PrayerGroupAdminUser {
                Id = 3,
                FullName = "Count Dooku",
                ImageFileId = 4,
                FileName = "count_dooku.jpg",
                FileType = FileType.Image,
            }
        ];
    }
}
