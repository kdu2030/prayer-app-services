using Moq;
using PrayerAppServices.PrayerGroups;
using PrayerAppServices.PrayerGroups.DTOs;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.Users;
using PrayerAppServices.Users.Models;
using Tests.MockData;

namespace Tests {
    public class PrayerGroupManagerTests {
        [Test]
        public void CreatePrayerGroup_GivenValidPrayerGroupRequest_CreatesPrayerGroup() {
            string username = "abernard";
            Mock<IUserManager> mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(userManager => userManager.ExtractUsernameFromAuthHeader(It.IsAny<string>()))
                .Returns(() => username);

            CreatePrayerGroupResponse response = new CreatePrayerGroupResponse {
                Id = 1,
                GroupName = "Dunder Mifflin",
                AdminUserId = 1,
                AdminFullName = "Andy Bernard",
            };
            PrayerGroupRequest newPrayerGroup = new PrayerGroupRequest {
                GroupName = "Dunder Mifflin"
            };

            Mock<IPrayerGroupRepository> mockRepository = new Mock<IPrayerGroupRepository>();
            mockRepository
                .Setup(mockRepository => mockRepository.CreatePrayerGroup(It.IsAny<string>(), It.IsAny<PrayerGroupDTO>()))
                .Returns(response);

            IPrayerGroupManager prayerGroupManager = new PrayerGroupManager(mockRepository.Object, mockUserManager.Object);
            PrayerGroupDetails details = prayerGroupManager.CreatePrayerGroup("mockToken", newPrayerGroup);

            Assert.Multiple(() => {
                Assert.That((details.GroupName ?? "").Equals(newPrayerGroup.GroupName), Is.True);
                Assert.That(details.Id, Is.Not.Null);
            });
        }

        [Test]
        public void CreatePrayerGroup_GivenValidImage_AddImageToPrayerGroup() {
            Mock<IUserManager> mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(userManager => userManager.ExtractUsernameFromAuthHeader(It.IsAny<string>()))
                .Returns(() => "mockUsername");
            string mockFileName = "group_image.png";
            string mockUrl = "http://127.0.0.1:5000/group_image.png";

            CreatePrayerGroupResponse response = new CreatePrayerGroupResponse {
                Id = 1,
                GroupName = "Dunder Mifflin",
                AdminUserId = 1,
                AdminFullName = "Andy Bernard",
                ImageFileId = 2,
                GroupImageFileName = mockFileName,
                GroupImageFileUrl = mockUrl
            };

            PrayerGroupRequest groupRequest = new PrayerGroupRequest {
                GroupName = "Dunder Mifflin",
                ImageFileId = 2,
            };

            Mock<IPrayerGroupRepository> mockRepository = new Mock<IPrayerGroupRepository>();
            mockRepository
                .Setup(mockRepository => mockRepository.CreatePrayerGroup(It.IsAny<string>(), It.IsAny<PrayerGroupDTO>()))
                .Returns(response);

            IPrayerGroupManager prayerGroupManager = new PrayerGroupManager(mockRepository.Object, mockUserManager.Object);
            PrayerGroupDetails details = prayerGroupManager.CreatePrayerGroup("mockToken", groupRequest);

            Assert.Multiple(() => {
                Assert.That(details.ImageFile?.Id, Is.EqualTo(2));
                Assert.That(details.ImageFile?.FileName, Is.EqualTo(mockFileName));
                Assert.That(details.ImageFile?.Url, Is.EqualTo(mockUrl));
            });

        }

        [Test]
        public void CreatePrayerGroup_GivenUser_AddsUserToAdminList() {
            string username = "abernard";
            Mock<IUserManager> mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(userManager => userManager.ExtractUsernameFromAuthHeader(It.IsAny<string>()))
                .Returns(() => username);

            CreatePrayerGroupResponse response = new CreatePrayerGroupResponse {
                Id = 1,
                GroupName = "Dunder Mifflin",
                AdminUserId = 1,
                AdminFullName = "Andy Bernard",
            };
            PrayerGroupRequest newPrayerGroup = new PrayerGroupRequest {
                GroupName = "Dunder Mifflin"
            };

            Mock<IPrayerGroupRepository> mockRepository = new Mock<IPrayerGroupRepository>();
            mockRepository
                .Setup(mockRepository => mockRepository.CreatePrayerGroup(It.IsAny<string>(), It.IsAny<PrayerGroupDTO>()))
                .Returns(response);

            IPrayerGroupManager prayerGroupManager = new PrayerGroupManager(mockRepository.Object, mockUserManager.Object);
            PrayerGroupDetails details = prayerGroupManager.CreatePrayerGroup("mockToken", newPrayerGroup);

            IEnumerable<UserSummary> adminUsers = details.Admins ?? [];
            Assert.That(adminUsers.Where(admin => admin.Id == response?.AdminUserId).Count, Is.EqualTo(1));
        }

        [Test]
        public void GetGroupDetails_GivenValidGroupId_FetchesGroupDetails() {
            string username = "abernard";
            Mock<IUserManager> mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(userManager => userManager.ExtractUsernameFromAuthHeader(It.IsAny<string>()))
                .Returns(() => username);

            Mock<IPrayerGroupRepository> mockRepository = new Mock<IPrayerGroupRepository>();
            mockRepository
                .Setup(repository => repository.GetPrayerGroupById(It.IsAny<int>()))
                .Returns(MockPrayerGroupData.MockPrayerGroup);

            IPrayerGroupManager manager = new PrayerGroupManager(mockRepository.Object, mockUserManager.Object);
            PrayerGroupDetails prayerGroupDetails = manager.GetPrayerGroupDetails("mockToken", 2);

            Assert.Multiple(() => {
                Assert.That(prayerGroupDetails.Id, Is.EqualTo(MockPrayerGroupData.MockPrayerGroup.Id));
                Assert.That(prayerGroupDetails.GroupName, Is.EqualTo(MockPrayerGroupData.MockPrayerGroup.GroupName));
            });
        }
    }
}
