

using PrayerAppServices.Files.Constants;
using PrayerAppServices.Files.Entities;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Entities;

namespace Tests.MockData {
    public static class MockPrayerGroupData {
        public static readonly IEnumerable<PrayerGroupUserEntity> MockPrayerGroupAdminUsers = [
            new PrayerGroupUserEntity {
                Id = 1,
                FullName = "Anakin Skywalker",
                GroupRole = PrayerGroupRole.Admin,
                ImageFileId = 1,
                FileName = "anakin_skywalker.png",
                FileType = FileType.Image,
            },
            new PrayerGroupUserEntity {
                Id = 2,
                FullName = "Obi Wan Kenobi"
            },
            new PrayerGroupUserEntity {
                Id = 3,
                FullName = "Count Dooku",
                ImageFileId = 4,
                FileName = "count_dooku.jpg",
                FileType = FileType.Image,
            }
        ];

        public static readonly PrayerGroup MockPrayerGroup = new PrayerGroup {
            Id = 2,
            GroupName = "IMB",
            Color = 65280,
            Description = "Missionary organization",
            Rules = "No explicit language",
            ImageFile = new MediaFile {
                Id = 1,
                FileName = "imb-logo.jpg",
                Url = "https://127.0.0.1:5000/static/4.jpg",
                FileType = FileType.Image,
            },
        };

        public static readonly PrayerGroupAppUser PrayerGroupAppUser = new PrayerGroupAppUser {
            Id = 6,
            FullName = "Commander Cody",
            ImageFileId = 4,
            FileName = "commander_cody.jpg",
            FileType = FileType.Image,
        };
    }
}
