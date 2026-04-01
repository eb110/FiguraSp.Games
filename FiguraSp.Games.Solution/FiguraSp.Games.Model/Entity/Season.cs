namespace FiguraSp.Games.Model.Entity
{
    public record Season
    {
        public Guid Id { get; set; }

        public required string Year { get; set; }
    }
}
