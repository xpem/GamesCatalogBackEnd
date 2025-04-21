using Models.DTOs;
using Models.Reqs;
using Models.Resps;
using Moq;
using Repos;
using Services;

namespace ServicesTests
{
    [TestClass()]
    public class GameStatusServiceTests
    {
        private readonly Mock<IGameRepo> _mockGameRepo;
        private readonly Mock<IGameStatusRepo> _mockGameStatusRepo;
        private readonly GameStatusService _service;

        public GameStatusServiceTests()
        {
            _mockGameRepo = new Mock<IGameRepo>();
            _mockGameStatusRepo = new Mock<IGameStatusRepo>();
            _service = new GameStatusService(_mockGameRepo.Object, _mockGameStatusRepo.Object);
        }

        [TestMethod()]
        public async Task UpsertAsync_ShouldCreateGameAndGameStatus_WhenGameAndGameStatusDoNotExist()
        {
            // Arrange
            var reqGame = new ReqGame
            {
                IGDBId = 123,
                Name = "Test Game",
                CoverUrl = "http://example.com/cover.jpg",
                Platforms = "PC",
                ReleaseDate = "2023-01-01",
                Summary = "Test Summary",
                Status = GameStatus.Playing,
                Rate = 5
            };

            var uid = 1;

            _mockGameRepo.Setup(repo => repo.GetByIGDBIdAsync(reqGame.IGDBId))
                .ReturnsAsync((GameDTO?)null);

            _mockGameStatusRepo.Setup(repo => repo.GetByGameIdAndUserIdAsync(It.IsAny<int>(), uid))
                .ReturnsAsync((GameStatusDTO?)null);

            _mockGameRepo.Setup(repo => repo.CreateAsync(It.IsAny<GameDTO>()))
                .Returns(Task.CompletedTask);

            _mockGameStatusRepo.Setup(repo => repo.CreateAsync(It.IsAny<GameStatusDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpsertAsync(reqGame, uid);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Content);
            _mockGameRepo.Verify(repo => repo.CreateAsync(It.IsAny<GameDTO>()), Times.Once);
            _mockGameStatusRepo.Verify(repo => repo.CreateAsync(It.IsAny<GameStatusDTO>()), Times.Once);
        }

        [TestMethod()]
        public async Task UpsertAsync_ShouldUpdateGameStatus_WhenGameStatusExists()
        {
            // Arrange
            var reqGame = new ReqGame
            {
                IGDBId = 123,
                Name = "Test Game",
                CoverUrl = "http://example.com/cover.jpg",
                Platforms = "PC",
                ReleaseDate = "2023-01-01",
                Summary = "Test Summary",
                Status = GameStatus.Playing,
                Rate = 5
            };

            var uid = 1;

            var existingGame = new GameDTO
            {
                Id = 1,
                IGDBId = reqGame.IGDBId,
                Name = reqGame.Name,
                CreatedAt = DateTime.UtcNow,
            };

            var existingGameStatus = new GameStatusDTO
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                GameId = existingGame.Id,
                UserId = uid,
                Status = GameStatus.Want,
                Rate = 3
            };

            _mockGameRepo.Setup(repo => repo.GetByIGDBIdAsync(reqGame.IGDBId))
                .ReturnsAsync(existingGame);

            _mockGameStatusRepo.Setup(repo => repo.GetByGameIdAndUserIdAsync(existingGame.Id, uid))
                .ReturnsAsync(existingGameStatus);

            _mockGameStatusRepo.Setup(repo => repo.UpdateAsync(It.IsAny<GameStatusDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpsertAsync(reqGame, uid);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Content);
            _mockGameRepo.Verify(repo => repo.CreateAsync(It.IsAny<GameDTO>()), Times.Never);
            _mockGameStatusRepo.Verify(repo => repo.UpdateAsync(It.IsAny<GameStatusDTO>()), Times.Once);
        }

        [TestMethod()]
        public async Task UpdateAsync_ShouldReturnValidationError_WhenRequestIsInvalid()
        {
            // Arrange
            var reqGameStatus = new ReqGameStatus { Id = 1, Status = GameStatus.Played, Rate = 5 };
            var uid = 1;
            _mockGameStatusRepo.Setup(repo => repo.GetByIdAndUserIdAsync(reqGameStatus.Id, uid))
                .ReturnsAsync((GameStatusDTO?)null);

            // Act
            var result = await _service.UpdateAsync(reqGameStatus, uid);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Game status not found", result.Error?.Message);
        }

        [TestMethod()]
        public async Task UpdateAsync_ShouldUpdateGameStatus_WhenRequestIsValid()
        {
            // Arrange
            var reqGameStatus = new ReqGameStatus { Id = 1, Status = GameStatus.Played, Rate = 5 };
            var uid = 1;
            var existingGameStatus = new GameStatusDTO
            {
                Id = reqGameStatus.Id,
                UserId = uid,
                Status = GameStatus.Playing,
                Rate = 3,
                CreatedAt = DateTime.UtcNow,
                GameId = 1
            };

            _mockGameStatusRepo.Setup(repo => repo.GetByIdAndUserIdAsync(reqGameStatus.Id, uid))
                .ReturnsAsync(existingGameStatus);
            _mockGameStatusRepo.Setup(repo => repo.UpdateAsync(It.IsAny<GameStatusDTO>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(reqGameStatus, uid);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Content);
            Assert.IsInstanceOfType<ResGameStatusIds>(result.Content);
            var resIds = (ResGameStatusIds)result.Content!;
            Assert.AreEqual(existingGameStatus.GameId, resIds.GameId);
            Assert.AreEqual(existingGameStatus.Id, resIds.GameStatusId);

            _mockGameStatusRepo.Verify(repo => repo.UpdateAsync(It.Is<GameStatusDTO>(gs =>
                gs.Status == reqGameStatus.Status && gs.Rate == reqGameStatus.Rate)), Times.Once);
        }
    }
}