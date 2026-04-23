using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FiguraSp.Games.Model.Requests
{
    public class GameEditRequestDto
    {
        public required Guid Id { get; set; }
        public required Guid StageId { get; set; }
        public required DateOnly GameDate { get; set; }
        [Precision(3, 1)]
        [Range(13, 76)]
        public required decimal HomeScore { get; set; }
        [Precision(3, 1)]
        [Range(13, 76)]
        public required decimal AwayScore { get; set; }
    }
}
