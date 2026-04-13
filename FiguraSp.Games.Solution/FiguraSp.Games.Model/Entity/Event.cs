using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiguraSp.Games.Model.Entity
{
    public record Event
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Game")]
        public required Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
        public required Guid RiderId { get; set; }
        public required int RiderGameNumber { get; set; } = 0;
        public required int RiderHeatNumber { get; set; } = 0;
        public required int RiderRowNumber { get; set; }
        public required string EventResult { get; set; }
        public required string HomeAway { get; set; }
    }
}
