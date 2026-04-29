using FiguraSp.Games.Model.Entity;
using FiguraSp.SharedLibrary.Responses;

namespace FiguraSp.Games.Service.Validators
{
    public static class GameValidator
    {
        public static DefaultResponse ValidateGameScore(Game game)
        {
            var homeScore = game.Events.Where(e => e.HomeAway.Equals("Home") && decimal.TryParse("" + e.EventResult[0], out _)).Sum(e => decimal.Parse("" + e.EventResult[0]));
            if (homeScore != game.HomeScore)
            {
                return new()
                {
                    Success = false,
                    Errors = ["Home score does not match the sum of home events."]
                };
            }

            var awayScore = game.Events.Where(e => e.HomeAway.Equals("Away") && decimal.TryParse("" + e.EventResult[0], out _)).Sum(e => decimal.Parse("" + e.EventResult[0]));
            if (awayScore != game.AwayScore)
            {
                return new()
                {
                    Success = false,
                    Errors = ["Away score does not match the sum of away events."]
                };
            }

            return new() { Success = true };
        }
    }
}
