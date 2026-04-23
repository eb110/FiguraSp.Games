using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiguraSp.Games.Model.Entity
{
    public record Game
    {
        [Key]
        public Guid Id { get; set; }
        public required Guid TeamHomeId { get; set; }
        public required Guid TeamAwayId { get; set; }
        [ForeignKey("Season")]
        public required Guid SeasonId { get; set; }
        public Season Season { get; set; } = null!;
        [ForeignKey("Level")]
        public required Guid LevelId { get; set; }
        public PicklistGameLevel Level { get; set; } = null!;
        [ForeignKey("Stage")]
        public Guid? StageId { get; set; }
        public PicklistGameStage Stage { get; set; } = null!;
        public required bool Inserted { get; set; } = false; 
        public required DateOnly GameDate { get; set; }
        public List<Event> Events { get; set; } = [];
        [Precision(3, 1)]
        [Range(0, 90)]
        public required decimal HomeScore { get; set; } = 0;
        [Precision(3, 1)]
        [Range(0, 90)]
        public required decimal AwayScore { get; set; } = 0;
    }
}
