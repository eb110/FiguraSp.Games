using FiguraSp.Games.Model.Entity;
using FiguraSp.Games.Model.Responses;

namespace FiguraSp.Games.Model.Extensions
{
    public static class SeasonExtension
    {
        public static SeasonResponseDto ToSeasonResponseDto(this Season season)
        {
            SeasonResponseDto response = new()
            {
                Id = season.Id,
                Year = season.Year,
            };

            return response;
        }
    }
}
