﻿using AutoMapper;
using Moq;
using PrayerAppServices.Files;
using PrayerAppServices.PrayerGroups;
using PrayerAppServices.PrayerGroups.DTOs;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Mappers;
using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.Users;
using PrayerAppServices.Users.Models;
using Tests.MockData;

namespace Tests {
    public class PrayerGroupManagerTests {
        private IMapper _mapper;
        private readonly Mock<IMediaFileRepository> _mockMediaFileRepository = new Mock<IMediaFileRepository>();
        private readonly Mock<IPrayerGroupRepository> _mockPrayerGroupRepository = new Mock<IPrayerGroupRepository>();
        private readonly Mock<IUserManager> _mockUserManager = new Mock<IUserManager>();

        [OneTimeSetUp]
        public void OneTimeSetUp() {
            MapperConfiguration config = new MapperConfiguration(cfg => {
                cfg.AddProfile<PrayerGroupMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [TearDown]
        public void TearDown() {
            _mockMediaFileRepository.Reset();
            _mockPrayerGroupRepository.Reset();
            _mockUserManager.Reset();
        }

        [Test]
        public async Task CreatePrayerGroup_GivenValidPrayerGroupRequest_CreatesPrayerGroup() {
            string username = "abernard";
            Mock<IUserManager> mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(userManager => userManager.ExtractUsernameFromAuthHeader(It.IsAny<string>()))
                .Returns(() => username);

            PrayerGroupDetailsEntity response = new PrayerGroupDetailsEntity {
                Id = 1,
                GroupName = "Dunder Mifflin",
                AdminUserId = 1,
                AdminFullName = "Andy Bernard",
            };
            PrayerGroupRequest newPrayerGroup = new PrayerGroupRequest {
                Description = "The best paper company in Scranton",
                GroupName = "Dunder Mifflin"
            };

            Mock<IPrayerGroupRepository> mockRepository = new Mock<IPrayerGroupRepository>();
            mockRepository
                .Setup(mockRepository => mockRepository.CreatePrayerGroupAsync(It.IsAny<string>(), It.IsAny<PrayerGroupDTO>()))
                .ReturnsAsync(response);

            IPrayerGroupManager prayerGroupManager = new PrayerGroupManager(mockRepository.Object, mockUserManager.Object, _mockMediaFileRepository.Object, _mapper);
            PrayerGroupDetails details = await prayerGroupManager.CreatePrayerGroupAsync("mockToken", newPrayerGroup);

            Assert.Multiple(() => {
                Assert.That((details.GroupName ?? "").Equals(newPrayerGroup.GroupName), Is.True);
                Assert.That(details.Id, Is.Not.Null);
            });
        }

        [Test]
        public async Task CreatePrayerGroup_GivenValidImage_AddImageToPrayerGroup() {
            Mock<IUserManager> mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(userManager => userManager.ExtractUsernameFromAuthHeader(It.IsAny<string>()))
                .Returns(() => "mockUsername");
            string mockFileName = "group_image.png";
            string mockUrl = "http://127.0.0.1:5000/group_image.png";

            PrayerGroupDetailsEntity response = new PrayerGroupDetailsEntity {
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
                Description = "The best paper company in Scranton",
                ImageFileId = 2,
            };

            Mock<IPrayerGroupRepository> mockRepository = new Mock<IPrayerGroupRepository>();
            mockRepository
                .Setup(mockRepository => mockRepository.CreatePrayerGroupAsync(It.IsAny<string>(), It.IsAny<PrayerGroupDTO>()))
                .ReturnsAsync(response);

            IPrayerGroupManager prayerGroupManager = new PrayerGroupManager(mockRepository.Object, mockUserManager.Object, _mockMediaFileRepository.Object, _mapper);
            PrayerGroupDetails details = await prayerGroupManager.CreatePrayerGroupAsync("mockToken", groupRequest);

            Assert.Multiple(() => {
                Assert.That(details.ImageFile?.Id, Is.EqualTo(2));
                Assert.That(details.ImageFile?.FileName, Is.EqualTo(mockFileName));
                Assert.That(details.ImageFile?.Url, Is.EqualTo(mockUrl));
            });

        }

        [Test]
        public async Task CreatePrayerGroup_GivenUser_AddsUserToAdminList() {
            string username = "abernard";
            Mock<IUserManager> mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(userManager => userManager.ExtractUsernameFromAuthHeader(It.IsAny<string>()))
                .Returns(() => username);

            PrayerGroupDetailsEntity response = new PrayerGroupDetailsEntity {
                Id = 1,
                GroupName = "Dunder Mifflin",
                AdminUserId = 1,
                AdminFullName = "Andy Bernard",
            };
            PrayerGroupRequest newPrayerGroup = new PrayerGroupRequest {
                GroupName = "Dunder Mifflin",
                Description = "The best paper company in Scranton",
            };

            Mock<IPrayerGroupRepository> mockRepository = new Mock<IPrayerGroupRepository>();
            mockRepository
                .Setup(mockRepository => mockRepository.CreatePrayerGroupAsync(It.IsAny<string>(), It.IsAny<PrayerGroupDTO>()))
                .ReturnsAsync(response);

            IPrayerGroupManager prayerGroupManager = new PrayerGroupManager(mockRepository.Object, mockUserManager.Object, _mockMediaFileRepository.Object, _mapper);
            PrayerGroupDetails details = await prayerGroupManager.CreatePrayerGroupAsync("mockToken", newPrayerGroup);

            IEnumerable<UserSummary> adminUsers = details.Admins ?? [];
            Assert.That(adminUsers.Where(admin => admin.Id == response?.AdminUserId).Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetGroupDetails_GivenValidGroupId_FetchesGroupDetails() {
            string username = "abernard";
            Mock<IUserManager> mockUserManager = new Mock<IUserManager>();
            mockUserManager
                .Setup(userManager => userManager.ExtractUsernameFromAuthHeader(It.IsAny<string>()))
                .Returns(() => username);

            Mock<IPrayerGroupRepository> mockRepository = new Mock<IPrayerGroupRepository>();

            mockRepository
                .Setup(repository => repository.GetPrayerGroupByIdAsync(It.IsAny<int>(), true))
                .ReturnsAsync(MockPrayerGroupData.MockPrayerGroup);

            IPrayerGroupManager manager = new PrayerGroupManager(mockRepository.Object, mockUserManager.Object, _mockMediaFileRepository.Object, _mapper);
            PrayerGroupDetails prayerGroupDetails = await manager.GetPrayerGroupDetailsAsync("mockToken", 2);

            Assert.Multiple(() => {
                Assert.That(prayerGroupDetails.Id, Is.EqualTo(MockPrayerGroupData.MockPrayerGroup.Id));
                Assert.That(prayerGroupDetails.GroupName, Is.EqualTo(MockPrayerGroupData.MockPrayerGroup.GroupName));
            });
        }

        [Test]
        public async Task UpdatePrayerGroupAsync_GivenValidPrayerGroupRequest_UpdatesPrayerGroup() {
            PrayerGroupRequest request = new PrayerGroupRequest {
                GroupName = "Dunder Mifflin",
                Description = "The best paper company in Scranton",
                Rules = "No horseplay",
                Color = "#ffffff",
                ImageFileId = 1
            };

            _mockMediaFileRepository.Setup(repository => repository.GetMediaFileByIdAsync(1)).ReturnsAsync(MockPrayerGroupData.MockMediaFile);
            _mockPrayerGroupRepository.Setup(repository => repository.UpdatePrayerGroupAsync(It.IsAny<PrayerGroup>())).Returns(Task.CompletedTask);

            IPrayerGroupManager manager = new PrayerGroupManager(_mockPrayerGroupRepository.Object, _mockUserManager.Object, _mockMediaFileRepository.Object, _mapper);
            PrayerGroupDetails updatedGroup = await manager.UpdatePrayerGroupAsync(1, request);

            Assert.Multiple(() => {
                Assert.That(updatedGroup.Id, Is.EqualTo(1));
                Assert.That(updatedGroup.GroupName, Is.EqualTo(request.GroupName));
                Assert.That(updatedGroup.Description, Is.EqualTo(request.Description));
            });
        }

        [Test]
        public void UpdatePrayerGroupAsync_GivenDuplicateGroupName_ThrowsException() {
            PrayerGroupRequest request = new PrayerGroupRequest {
                GroupName = "Dunder Mifflin",
                Description = "The best paper company in Scranton",
                Rules = "No horseplay",
                Color = "#ffffff",
                ImageFileId = 1
            };

            PrayerGroup existingPrayerGroup = new PrayerGroup { Id = 2, GroupName = "Dunder Mifflin", Description = "Group Name Description" };

            _mockMediaFileRepository.Setup(repository => repository.GetMediaFileByIdAsync(1)).ReturnsAsync(MockPrayerGroupData.MockMediaFile);
            _mockPrayerGroupRepository.Setup(repository => repository.UpdatePrayerGroupAsync(It.IsAny<PrayerGroup>())).Returns(Task.CompletedTask);
            _mockPrayerGroupRepository.Setup(repository => repository.GetPrayerGroupByName("Dunder Mifflin")).Returns(existingPrayerGroup);

            IPrayerGroupManager manager = new PrayerGroupManager(_mockPrayerGroupRepository.Object, _mockUserManager.Object, _mockMediaFileRepository.Object, _mapper);
            Assert.ThrowsAsync<ArgumentException>(() => manager.UpdatePrayerGroupAsync(1, request));
        }

        [Test]
        public async Task UpdatePrayerGroupAsync_OmitsOptionalFields_UpdatesPrayerGroup() {
            PrayerGroupRequest request = new PrayerGroupRequest {
                GroupName = "Dunder Mifflin",
                Description = "The best paper company in Scranton",
            };

            _mockPrayerGroupRepository.Setup(repository => repository.UpdatePrayerGroupAsync(It.IsAny<PrayerGroup>())).Returns(Task.CompletedTask);

            IPrayerGroupManager manager = new PrayerGroupManager(_mockPrayerGroupRepository.Object, _mockUserManager.Object, _mockMediaFileRepository.Object, _mapper);
            PrayerGroupDetails updatedGroup = await manager.UpdatePrayerGroupAsync(1, request);

            Assert.Multiple(() => {
                Assert.That(updatedGroup.Id, Is.EqualTo(1));
                Assert.That(updatedGroup.GroupName, Is.EqualTo(request.GroupName));
                Assert.That(updatedGroup.Description, Is.EqualTo(request.Description));
            });
        }

    }
}