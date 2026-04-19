using FiguraSp.Games.Model.Data;
using FiguraSp.Games.Model.Entity;
using FiguraSp.Games.Model.Requests;
using FiguraSp.Games.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FiguraSp.Games.Test
{
    [TestClass]
    public class GameServiceTest
    {
        #region Variables

        private readonly GameService service;
        private readonly Mock<GamesDbContext> mockContext;
        private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
        private readonly Mock<DbSet<Game>> mockGame;

        private RiderEventsRequestDto riderEventsRequestDto;

        #endregion

        #region Controller

        public GameServiceTest()
        {
            DbContextOptions<GamesDbContext> options = new();
            mockContext = new Mock<GamesDbContext>(options);
            mockHttpClientFactory = new Mock<IHttpClientFactory>();
            service = new GameService(mockContext.Object, mockHttpClientFactory.Object);

            mockGame = MockGameContext.GetMockGameEntity();

            riderEventsRequestDto = new()
            {
                GameId = Guid.NewGuid(),
                RiderId = Guid.NewGuid(),
                GameRiderNr = 1,
                GameRiderResult = "0,1,2,3,-,u,w,d",
                HomeAway = "Away"
            };
        }

        #endregion

        #region Initialize

        [TestInitialize]
        public void Initialize()
        {
            Game gameEntity = new()
            {
                Id = Guid.NewGuid(),
                TeamAwayId = Guid.NewGuid(),
                TeamHomeId = Guid.NewGuid(),
                SeasonId = Guid.NewGuid(),
                LevelId = Guid.NewGuid(),
                Inserted = false,
                GameDate = new DateOnly(2025, 1, 1)
            };

            mockContext.Setup(c => c.Game).Returns(mockGame.Object);
            mockContext.Setup(x => x.GetEntitiesToListAsync(It.IsAny<IQueryable<Game>>()))
                .Returns(Task.FromResult(new List<Game> { gameEntity }));
            mockContext.Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<IQueryable<Game>>()))
                .Returns(Task.FromResult( gameEntity ));
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public async Task AddRiderEvents_WithProperContent_ReturnsSuccess()
        {
            //Act
            var result = await service.AddRiderEvents(riderEventsRequestDto);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public async Task AddRiderEvents_WithInproperContent_ReturnsError()
        {
            riderEventsRequestDto.GameRiderResult = "zd";
            //Act
            var result = await service.AddRiderEvents(riderEventsRequestDto);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, result.Errors.Count);
        }

        #endregion
    }
}
