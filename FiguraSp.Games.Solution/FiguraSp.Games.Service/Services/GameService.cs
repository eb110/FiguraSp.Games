using FiguraSp.Games.Model.Data;
using FiguraSp.Games.Model.Entity;
using FiguraSp.Games.Model.Extensions;
using FiguraSp.Games.Model.Responses;
using Microsoft.EntityFrameworkCore;

namespace FiguraSp.Games.Service.Services
{
    public class GameService(GamesDbContext context) : IGameService
    {
        public async Task<List<SeasonResponseDto>> GetSeasons()
        {
            IQueryable<Season> query = context.Seasons.AsQueryable().AsNoTracking();
            var seasons = await context.GetEntitiesToListAsync(query);
            List<SeasonResponseDto> result = [.. seasons.Select(x => x.ToSeasonResponseDto())];
            return result;
        }
    }

    public interface IGameService
    {
        public Task<List<SeasonResponseDto>> GetSeasons();
    }
}
