using FiguraSp.SharedLibrary.Responses;

namespace FiguraSp.Games.Model.Responses
{
    public record EventResponseDto : DefaultResponse
    {
        public Guid? Id { get; set; }
        public Guid? GameId { get; set; }
        public Guid? RiderId { get; set; }
        public int? RiderGameNumber { get; set; }
        public int? RiderHeatNumber { get; set; }
        public int? RiderRowNumber { get; set; }
        public string? EventResult { get; set; }
        public string? HomeAway { get; set; }
    }
}
