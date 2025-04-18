
using Moq;
using PrayerAppServices.PrayerGroups;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerRequests;
using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.PrayerRequests.Models;
using Tests.MockData;

namespace Tests {
    public class PrayerRequestManagerTests {
        private readonly Mock<IPrayerGroupRepository> _mockPrayerGroupRepository = new Mock<IPrayerGroupRepository>();
        private readonly Mock<IPrayerRequestRepository> _mockPrayerRequestRepository = new Mock<IPrayerRequestRepository>();

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

            PrayerRequestManager prayerRequestManager = new PrayerRequestManager(_mockPrayerRequestRepository.Object, _mockPrayerGroupRepository.Object);
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

            PrayerRequestManager prayerRequestManager = new PrayerRequestManager(_mockPrayerRequestRepository.Object, _mockPrayerGroupRepository.Object);

            Assert.ThrowsAsync<InvalidOperationException>(async () => {
                await prayerRequestManager.CreatePrayerRequestAsync(1, createRequest, CancellationToken.None);
            });
        }

    }
}
