

using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.Users.Entities;

namespace Tests.MockData {
    public static class MockPrayerRequestData {
        public static readonly PrayerGroupUser MockPrayerGroupUser = new PrayerGroupUser {
            Id = 1,
            AppUser = new AppUser {
                Id = 2,
                FullName = "Anakin Skywalker",
            },
            Role = PrayerGroupRole.Member,
            PrayerGroup = new PrayerGroup {
                Id = 3,
                GroupName = "IMB",
                Color = 65280,
                Description = "Missionary organization",
                Rules = "No explicit language",
            },
        };

        public static readonly PrayerRequest MockPrayerRequest = new PrayerRequest {
            Id = 34,
            RequestTitle = "Test Prayer Request",
            RequestDescription = "This is a test prayer request description.",
            CreatedDate = DateTime.UtcNow,
            PrayerGroup = new PrayerGroup {
                Id = 3,
                GroupName = "IMB",
                Color = 65280,
                Description = "Missionary organization",
                Rules = "No explicit language",
            },
            User = new AppUser {
                Id = 2,
                FullName = "Anakin Skywalker",
            },
            LikeCount = 10,
            PrayedCount = 3,
            ExpirationDate = DateTime.UtcNow.AddDays(7),
        };
    }
}
