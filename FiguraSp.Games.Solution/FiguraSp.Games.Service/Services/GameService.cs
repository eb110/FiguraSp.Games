using FiguraSp.Games.Model.Data;
using FiguraSp.Games.Model.Entity;
using FiguraSp.Games.Model.Extensions;
using FiguraSp.Games.Model.Requests;
using FiguraSp.Games.Model.Responses;
using FiguraSp.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace FiguraSp.Games.Service.Services
{
    public class GameService(GamesDbContext context, IHttpClientFactory httpClientFactory) : IGameService
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

        public async Task<DefaultResponse> AddGamesList(GamesRequestDto gamesRequest)
        {
            var client = httpClientFactory.CreateClient("figuraHttp");
            var payload = JsonConvert.SerializeObject(gamesRequest.TeamIds);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/team/CheckTeams", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var validateTeams = JsonConvert.DeserializeObject<DefaultResponse>(responseString);
            if(!validateTeams!.Success)
            {
                return validateTeams;
            }

            var validateSeason = await GetSeasonById(gamesRequest.SeasonId);
            if (!validateSeason.Success)
            {
                return validateTeams;
            }

            IQueryable<PicklistGameLevel> query = context.PicklistGameLevel.Where(x => x.Id.Equals(gamesRequest.GameLevelId)).AsQueryable();
            var validateLevel = await context.GetFirstOrDefaultAsync(query);
            if (validateLevel == null)
            {
                return new() { Errors = ["invalid game level"] };
            }

            List<Game> games = [];

            for(int i = 0; i < gamesRequest.TeamIds.Count; i++)
            {
                for(int j = 0; j < gamesRequest.TeamIds.Count; j++)
                {
                    if(i != j)
                    {
                        games.Add(new() 
                        { 
                            TeamHomeId = gamesRequest.TeamIds[i], 
                            TeamAwayId = gamesRequest.TeamIds[j],
                            SeasonId = gamesRequest.SeasonId,
                            LevelId = gamesRequest.GameLevelId,
                            Inserted = false
                        });
                    }
                }
            }

            try
            {
                context.AddRange(games);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                return new() { Errors = [ex.Message] };            
            }

            return new() { Success = true };
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

        public Task<DefaultResponse> AddGamesList(GamesRequestDto games);
    }
}
