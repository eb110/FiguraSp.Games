using FiguraSp.Games.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FiguraSp.Games.Test
{
    public class MockGameContext
    {
        public static Mock<DbSet<Game>> GetMockGameEntity()
        {
            var gameModel = new List<Game>
            { 
                new()
                {
                Id = Guid.NewGuid(),
                TeamAwayId = Guid.NewGuid(),
                TeamHomeId = Guid.NewGuid(),
                SeasonId = Guid.NewGuid(),
                LevelId = Guid.NewGuid(),
                Inserted = false,
                GameDate = new DateOnly(2025, 1, 1)
                }
            }.AsQueryable();

            var mockGame = new Mock<DbSet<Game>>();

            mockGame.As<IQueryable<Game>>().Setup(m => m.Provider).Returns(gameModel.Provider);
            mockGame.As<IQueryable<Game>>().Setup(m => m.Expression).Returns(gameModel.Expression);
            mockGame.As<IQueryable<Game>>().Setup(m => m.ElementType).Returns(gameModel.ElementType);
            mockGame.As<IQueryable<Game>>().Setup(m => m.GetEnumerator()).Returns(gameModel.GetEnumerator());

            return mockGame;
        }
    }
}
