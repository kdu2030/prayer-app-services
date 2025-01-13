﻿using Moq;
using PrayerAppServices.PrayerGroups;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.Users;

namespace Tests {
    public class PrayerGroupManagerTests {
        [Test]
        public void CreatePrayerGroup_GivenValidUser_CreatesPrayerGroup() {
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
            NewPrayerGroupRequest newPrayerGroup = new NewPrayerGroupRequest {
                GroupName = "Dunder Mifflin"
            };

            Mock<IPrayerGroupRepository> mockRepository = new Mock<IPrayerGroupRepository>();
            mockRepository
                .Setup(mockRepository => mockRepository.CreatePrayerGroup(It.IsAny<string>(), It.IsAny<NewPrayerGroup>()))
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

            NewPrayerGroupRequest groupRequest = new NewPrayerGroupRequest {
                GroupName = "Dunder Mifflin",
                ImageFileId = 2,
            };

            Mock<IPrayerGroupRepository> mockRepository = new Mock<IPrayerGroupRepository>();
            mockRepository
                .Setup(mockRepository => mockRepository.CreatePrayerGroup(It.IsAny<string>(), It.IsAny<NewPrayerGroup>()))
                .Returns(response);

            IPrayerGroupManager prayerGroupManager = new PrayerGroupManager(mockRepository.Object, mockUserManager.Object);
            PrayerGroupDetails details = prayerGroupManager.CreatePrayerGroup("mockToken", groupRequest);

            Assert.Multiple(() => {
                Assert.That(details.ImageFile?.Id, Is.EqualTo(2));
                Assert.That(details.ImageFile?.FileName, Is.EqualTo(mockFileName));
                Assert.That(details.ImageFile?.Url, Is.EqualTo(mockUrl));
            });

        }
    }
}
