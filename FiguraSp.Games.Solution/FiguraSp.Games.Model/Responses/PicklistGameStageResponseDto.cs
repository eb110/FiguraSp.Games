using FiguraSp.SharedLibrary.Responses;

namespace FiguraSp.Games.Model.Responses
{
    public record PicklistGameStageResponseDto : DefaultResponse
    {
        public Guid? Id { get; set; }

        public string? GameStage { get; set; }
    }
}
