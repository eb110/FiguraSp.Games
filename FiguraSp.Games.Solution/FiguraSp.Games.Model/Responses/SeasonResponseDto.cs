using FiguraSp.SharedLibrary.Responses;

namespace FiguraSp.Games.Model.Responses
{
    public record SeasonResponseDto : DefaultResponse
    {
        public Guid? Id { get; set; }

        public string? Year { get; set; }
    }
}
