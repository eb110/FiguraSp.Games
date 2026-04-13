using FiguraSp.Games.Model.Views;
using FiguraSp.SharedLibrary.Responses;

namespace FiguraSp.Games.Model.Responses
{
    public record GameRiderEventsResponseDto : DefaultResponse
    {
        public List<VRiderEvent> GameRiderEvents { get; set; } = [];
    }
}
