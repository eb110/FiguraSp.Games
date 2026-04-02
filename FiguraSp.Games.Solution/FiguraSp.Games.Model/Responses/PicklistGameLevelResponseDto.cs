using FiguraSp.SharedLibrary.Responses;

namespace FiguraSp.Games.Model.Responses
{
    public record PicklistGameLevelResponseDto : DefaultResponse
    {
        public Guid? Id { get; set; }

        public string? GameLevel { get; set; }
    }
}
