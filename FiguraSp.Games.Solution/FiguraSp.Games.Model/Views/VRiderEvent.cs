using System.ComponentModel.DataAnnotations;

namespace FiguraSp.Games.Model.Views;

public partial class VRiderEvent
{
    [Key]
    public Guid RiderId { get; set; }

    public Guid GameId { get; set; }

    public int RiderGameNumber { get; set; }

    public string HomeAway { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateOnly DoB { get; set; }

    public string? Result { get; set; }

    public string? Heats { get; set; }

    public string? Rows { get; set; }
}
