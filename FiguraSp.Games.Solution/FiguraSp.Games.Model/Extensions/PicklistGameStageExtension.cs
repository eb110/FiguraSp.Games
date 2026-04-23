using FiguraSp.Games.Model.Entity;
using FiguraSp.Games.Model.Responses;

namespace FiguraSp.Games.Model.Extensions
{
    public static class PicklistGameStageExtension
    {
        public static PicklistGameStageResponseDto ToPicklistResponseDto(this PicklistGameStage stage)
        {
            PicklistGameStageResponseDto response = new()
            {
                Id = stage.Id,
                GameStage = stage.GameStage,
                Success = true
            };

            return response;
        }
    }
}
