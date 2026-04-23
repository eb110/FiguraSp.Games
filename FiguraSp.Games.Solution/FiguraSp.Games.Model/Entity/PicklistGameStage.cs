using System.ComponentModel.DataAnnotations;

namespace FiguraSp.Games.Model.Entity
{
    public record PicklistGameStage
    {
        [Key]
        public Guid Id { get; set; }

        public required string GameStage { get; set; }
    }
}
