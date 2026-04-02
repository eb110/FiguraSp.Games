using FiguraSp.Games.Model.Data;
using FiguraSp.Games.Model.Entity;
using FiguraSp.Games.Model.Extensions;
using FiguraSp.Games.Model.Responses;
using Microsoft.EntityFrameworkCore;

namespace FiguraSp.Games.Service.Services
{
    public class GameService(GamesDbContext context) : IGameService
    {
        public async Task<SeasonResponseDto> AddSeason(string year)
        {
            var seasonExist = await GetSeasonByYear(year);
            if(seasonExist.Success)
            {
                return new() { Errors = [$"Season of {year} already exist"] };
            }

            var season = new Season { Year = year };

            context.Seasons.Add(season);
            var check = await context.SaveChangesAsync();

            if (check != 1)
            {
                return new() { Errors = ["Failed to add a new season"] };
            }

            return new() { Success = true, Year = year };
        }

        public async Task<List<PicklistGameLevelResponseDto>> GetLevels()
        {
            IQueryable<PicklistGameLevel> query = context.PicklistGameLevel.OrderBy(x => x.GameLevel).AsQueryable().AsNoTracking();
            var picklist = await context.GetEntitiesToListAsync(query);
            List<PicklistGameLevelResponseDto> result = [.. picklist.Select(x => x.ToPicklistResponseDto())];
            return result;
        }

        public async Task<SeasonResponseDto> GetSeasonById(Guid id)
        {
            IQueryable<Season> query = context.Seasons.Where(s => s.Id.Equals(id)).AsQueryable();
            var season = await context.GetFirstOrDefaultAsync(query);
            if (season == null)
            {
                return new() { Errors = ["Season not found"] };
            }
            return new() { Success = true, Id = season.Id, Year = season.Year };
        }

        public async Task<SeasonResponseDto> GetSeasonByYear(string year)
        {
            IQueryable<Season> query = context.Seasons.Where(s => s.Year.Equals(year)).AsQueryable();
            var season = await context.GetFirstOrDefaultAsync(query);
            if (season == null)
            {
                return new() { Errors = ["Season not found"] };
            }
            return new() { Success = true, Id = season.Id, Year = season.Year};
        }

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
        public Task<List<PicklistGameLevelResponseDto>> GetLevels();
        public Task<SeasonResponseDto> AddSeason(string year);
        public Task<SeasonResponseDto> GetSeasonByYear(string year);
        public Task<SeasonResponseDto> GetSeasonById(Guid id);
    }
}
