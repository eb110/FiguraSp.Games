using FiguraSp.Riders.Model.DTOs.Responses;

namespace FiguraSp.Games.Model.Responses
{
    public record EventWithRiderResponseDto
    {
        public RiderResponseDto? RiderResponseDto { get; set; }
        public EventResponseDto? EventResponseDto { get; set; }
    }
}
