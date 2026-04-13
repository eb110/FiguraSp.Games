namespace FiguraSp.Games.Model.Requests
{
    public class RiderEventsRequestDto
    {
        public required Guid GameId { get; set; }
        public required Guid RiderId { get; set; }
        public required int GameRiderNr { get; set; }
        public required string GameRiderResult { get; set; }
        public required string HomeAway { get; set; }
    }
}
