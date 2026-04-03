using FiguraSp.Games.Model.Entity;
using FiguraSp.Games.Model.Responses;

namespace FiguraSp.Games.Model.Extensions
{
    public static class GameExtension
    {
        public static GamesResponseDto ToGamesResponseDto(this Game game)
        {
            GamesResponseDto response = new()
            {
                Id = game.Id,
                TeamHomeId = game.TeamHomeId,
                TeamAwayId = game.TeamAwayId,
                SeasonId = game.SeasonId,
                LevelId = game.LevelId,
                Inserted = game.Inserted,
                Success = true
            };

            return response;
        }
    }
}
