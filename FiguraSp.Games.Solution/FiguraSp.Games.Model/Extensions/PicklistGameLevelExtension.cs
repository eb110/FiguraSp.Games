using FiguraSp.Games.Model.Entity;
using FiguraSp.Games.Model.Responses;

namespace FiguraSp.Games.Model.Extensions
{
    public static class PicklistGameLevelExtension
    {
        public static PicklistGameLevelResponseDto ToPicklistResponseDto(this PicklistGameLevel level)
        {
            PicklistGameLevelResponseDto response = new()
            {
                Id = level.Id,
                GameLevel = level.GameLevel,
                Success = true
            };

            return response;
        }
    }
}
