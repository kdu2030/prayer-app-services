
using AutoMapper;
using Moq;
using PrayerAppServices.Common.Sorting;
using PrayerAppServices.PrayerGroups;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Mappers;
using PrayerAppServices.PrayerRequests;
using PrayerAppServices.PrayerRequests.Constants;
using PrayerAppServices.PrayerRequests.DTOs;
using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.PrayerRequests.Mappers;
using PrayerAppServices.PrayerRequests.Models;
using PrayerAppServices.Users.Mappers;
using Tests.MockData;

namespace Tests {
    public class PrayerRequestManagerTests {
        private readonly Mock<IPrayerGroupRepository> _mockPrayerGroupRepository = new Mock<IPrayerGroupRepository>();
        private readonly Mock<IPrayerRequestRepository> _mockPrayerRequestRepository = new Mock<IPrayerRequestRepository>();
        private IMapper _mapper;

        [SetUp]
        public void Setup() {
            MapperConfiguration configuration = new MapperConfiguration(cfg => {
                cfg.AddProfile<UserMappingProfile>();
                cfg.AddProfile<PrayerGroupMappingProfile>();
                cfg.AddProfile<PrayerRequestModelMappingProfile>();

            });
            _mapper = configuration.CreateMapper();
        }

        [TearDown]
        public void TearDown() {
            _mockPrayerGroupRepository.Reset();
            _mockPrayerRequestRepository.Reset();
        }

        [Test]
        public async Task CreatePrayerRequestAsync_GivenValidPrayerRequest_CreatesPrayerRequest() {
            PrayerRequest? createdPrayerRequest = null;

            _mockPrayerGroupRepository
                .Setup(repo => repo.GetPrayerGroupUserByUserIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockPrayerRequestData.MockPrayerGroupUser);

            _mockPrayerRequestRepository
                .Setup(repo => repo.CreatePrayerRequestAsync(It.IsAny<PrayerRequest>(), It.IsAny<CancellationToken>()))
                .Callback<PrayerRequest, CancellationToken>((prayerRequest, token) => {
                    createdPrayerRequest = prayerRequest;
                });

            PrayerRequestCreateRequest createRequest = new PrayerRequestCreateRequest {
                UserId = 1,
                RequestTitle = "Test Title",
                RequestDescription = "Test Description",
                ExpirationDate = DateTime.UtcNow.AddDays(1)
            };

            PrayerRequestManager prayerRequestManager = new PrayerRequestManager(_mockPrayerRequestRepository.Object, _mockPrayerGroupRepository.Object, _mapper);
            await prayerRequestManager.CreatePrayerRequestAsync(1, createRequest, CancellationToken.None);

            if (createdPrayerRequest == null) {
                Assert.Fail("Created prayer request is null.");
            }

            Assert.Multiple(() => {
                Assert.That(createdPrayerRequest?.RequestTitle, Is.EqualTo(createRequest.RequestTitle));
                Assert.That(createdPrayerRequest?.RequestDescription, Is.EqualTo(createRequest.RequestDescription));
                Assert.That(createdPrayerRequest?.User?.Id, Is.EqualTo(createRequest.UserId));
            });
        }

        [Test]
        public void CreatePrayerRequestAsync_UserNotInGroup_ThrowsInvalidOperationException() {
            _mockPrayerGroupRepository
                .Setup(repo => repo.GetPrayerGroupUserByUserIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PrayerGroupUser?)null);

            PrayerRequestCreateRequest createRequest = new PrayerRequestCreateRequest {
                UserId = 1,
                RequestTitle = "Test Title",
                RequestDescription = "Test Description"
            };

            PrayerRequestManager prayerRequestManager = new PrayerRequestManager(_mockPrayerRequestRepository.Object, _mockPrayerGroupRepository.Object, _mapper);

            Assert.ThrowsAsync<InvalidOperationException>(async () => {
                await prayerRequestManager.CreatePrayerRequestAsync(1, createRequest, CancellationToken.None);
            });
        }

        [Test]
        public async Task GetPrayerRequestsAsync_GivenValidFilterCriteria_ReturnsPrayerRequests() {
            PrayerRequestResponseDTO prayerRequestResponse = new PrayerRequestResponseDTO {
                PrayerRequests = new List<PrayerRequest> { MockPrayerRequestData.MockPrayerRequest },
                TotalCount = 1
            };

            _mockPrayerRequestRepository
              .Setup(repo => repo.GetPrayerRequestsAsync(It.IsAny<PrayerRequestFilterCriteria>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(prayerRequestResponse);

            _mockPrayerRequestRepository
                .Setup(repo => repo.GetPrayerRequestUserDataAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockPrayerRequestData.MockUserPrayerRequestData);
            PrayerRequestManager prayerRequestManager = new PrayerRequestManager(_mockPrayerRequestRepository.Object, _mockPrayerGroupRepository.Object, _mapper);

            PrayerRequestFilterRequest filterRequest = new PrayerRequestFilterRequest {
                FilterCriteria = new PrayerRequestFilterCriteria {
                    PrayerGroupIds = new List<int> { 3 },
                    CreatorUserIds = new List<int> { 2 },
                    PageIndex = 0,
                    PageSize = 10,
                    SortConfig = new SortConfig {
                        SortField = PrayerRequestSortFields.CreatedAt,
                        SortOrder = SortOrder.Descending,
                    },
                    IncludeExpiredRequests = false,

                },
                UserId = 2
            };

            PrayerRequestGetResponse prayerRequestGetResponse = await prayerRequestManager.GetPrayerRequestsAsync(filterRequest, CancellationToken.None);
            PrayerRequestModel? prayerRequest = prayerRequestGetResponse.PrayerRequests.FirstOrDefault();

            Assert.That(prayerRequest, Is.Not.Null);
            Assert.That(prayerRequest?.RequestTitle, Is.EqualTo(MockPrayerRequestData.MockPrayerRequest.RequestTitle));
        }

        [Test]
        public async Task GetPrayerRequestsAsync_GivenEmptyLikedIds_ReturnsPrayerRequests() {
            UserPrayerRequestData userPrayerRequestData = new UserPrayerRequestData {
                UserLikedRequestIds = new List<int?>(),
                UserCommentedPrayerRequestIds = new List<int?>()
            };

            PrayerRequestResponseDTO queryResponse = new PrayerRequestResponseDTO {
                PrayerRequests = new List<PrayerRequest> { MockPrayerRequestData.MockPrayerRequest },
                TotalCount = 1
            };

            _mockPrayerRequestRepository
              .Setup(repo => repo.GetPrayerRequestsAsync(It.IsAny<PrayerRequestFilterCriteria>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(queryResponse);

            _mockPrayerRequestRepository
                .Setup(repo => repo.GetPrayerRequestUserDataAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userPrayerRequestData);

            PrayerRequestManager prayerRequestManager = new PrayerRequestManager(_mockPrayerRequestRepository.Object, _mockPrayerGroupRepository.Object, _mapper);

            PrayerRequestFilterRequest filterRequest = new PrayerRequestFilterRequest {
                FilterCriteria = new PrayerRequestFilterCriteria {
                    PrayerGroupIds = new List<int> { 3 },
                    CreatorUserIds = new List<int> { 2 },
                    PageIndex = 0,
                    PageSize = 10,
                    SortConfig = new SortConfig {
                        SortField = PrayerRequestSortFields.CreatedAt,
                        SortOrder = SortOrder.Descending,
                    },
                    IncludeExpiredRequests = false,

                },
                UserId = 2
            };

            PrayerRequestGetResponse prayerRequestsResponse = await prayerRequestManager.GetPrayerRequestsAsync(filterRequest, CancellationToken.None);
            PrayerRequestModel? prayerRequest = prayerRequestsResponse.PrayerRequests.FirstOrDefault();

            Assert.That(prayerRequest?.IsUserLiked, Is.False);
        }
    }
}
