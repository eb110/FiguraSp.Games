using System.ComponentModel.DataAnnotations;

namespace FiguraSp.Games.Model.Entity
{
    public record PicklistGameLevel
    {
        [Key]
        public Guid Id { get; set; }

        public required string GameLevel { get; set; }
    }
}
