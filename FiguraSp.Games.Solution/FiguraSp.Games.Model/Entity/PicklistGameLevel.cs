namespace FiguraSp.Games.Model.Entity
{
    public record PicklistGameLevel
    {
        public Guid Id { get; set; }

        public required string GameLevel { get; set; }
    }
}
