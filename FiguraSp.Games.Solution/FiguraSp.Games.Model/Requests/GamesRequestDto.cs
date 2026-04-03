namespace FiguraSp.Games.Model.Requests
{
    public class GamesRequestDto
    {
        public required List<Guid> TeamIds { get; set; }
        public required Guid SeasonId { get; set; }
        public required Guid GameLevelId { get; set; }
    }
}
