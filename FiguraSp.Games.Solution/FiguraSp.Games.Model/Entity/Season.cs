using System.ComponentModel.DataAnnotations;

namespace FiguraSp.Games.Model.Entity
{
    public record Season
    {
        [Key]
        public Guid Id { get; set; }
        public required string Year { get; set; }

        public List<Game> Games { get; set; } = [];
    }
}
