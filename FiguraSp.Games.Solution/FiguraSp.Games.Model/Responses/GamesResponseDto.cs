using FiguraSp.SharedLibrary.Responses;

namespace FiguraSp.Games.Model.Responses
{
    public record GamesResponseDto : DefaultResponse
    {
        public Guid? Id { get; set; }
        public Guid? TeamHomeId { get; set; }
        public Guid? TeamAwayId { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? LevelId { get; set; }
        public bool? Inserted { get; set; }
        public DateOnly? GameDate { get; set; }
    }
}
