using FiguraSp.Games.Model.Data;
using FiguraSp.Games.Model.Entity;
using FiguraSp.Games.Model.Extensions;
using FiguraSp.Games.Model.Requests;
using FiguraSp.Games.Model.Responses;
using FiguraSp.Games.Model.Views;
using FiguraSp.Riders.Model.DTOs.Responses;
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
            if (seasonExist.Success)
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

        //frontend checkbox list of teams for current season
        public async Task<DefaultResponse> AddGamesList(GamesRequestDto gamesRequest)
        {
            var client = httpClientFactory.CreateClient("figuraHttp");
            var payload = JsonConvert.SerializeObject(gamesRequest.TeamIds);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/team/CheckTeams", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var validateTeams = JsonConvert.DeserializeObject<DefaultResponse>(responseString);
            if (!validateTeams!.Success)
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

            for (int i = 0; i < gamesRequest.TeamIds.Count; i++)
            {
                for (int j = 0; j < gamesRequest.TeamIds.Count; j++)
                {
                    if (i != j)
                    {
                        IQueryable<Game> gameQuery = context.Game
                            .Where(x => x.SeasonId.Equals(gamesRequest.SeasonId) &&
                            x.TeamHomeId.Equals(gamesRequest.TeamIds[i]) && x.TeamAwayId.Equals(gamesRequest.TeamIds[j]) &&
                            x.LevelId.Equals(gamesRequest.GameLevelId)).AsQueryable().AsNoTracking();

                        var gameCheck = await context.GetFirstOrDefaultAsync(gameQuery);
                        if (gameCheck is null)
                        {
                            games.Add(new()
                            {
                                TeamHomeId = gamesRequest.TeamIds[i],
                                TeamAwayId = gamesRequest.TeamIds[j],
                                SeasonId = gamesRequest.SeasonId,
                                LevelId = gamesRequest.GameLevelId,
                                GameDate = DateOnly.Parse($"{validateSeason.Year}-01-01"),
                                Inserted = false
                            });
                        }
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
            return new() { Success = true, Id = season.Id, Year = season.Year };
        }

        public async Task<List<SeasonResponseDto>> GetSeasons()
        {
            IQueryable<Season> query = context.Seasons.AsQueryable().AsNoTracking();
            var seasons = await context.GetEntitiesToListAsync(query);
            List<SeasonResponseDto> result = [.. seasons.Select(x => x.ToSeasonResponseDto())];
            return result;
        }

        public async Task<List<GamesResponseDto>> GetGamesBySeasonId(Guid id)
        {
            IQueryable<Game> query = context.Game.Where(x => x.SeasonId == id).AsQueryable().AsNoTracking();
            try
            {
                var games = await context.GetEntitiesToListAsync(query);
                List<GamesResponseDto> result = [.. games.Select(x => x.ToGamesResponseDto())];
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"can't fetch games: {ex.Message}");
            }
        }

        public async Task<GamesResponseDto> GetGameById(Guid id)
        {
            IQueryable<Game> query = context.Game.Where(x => x.Id.Equals(id)).AsQueryable().AsNoTracking();

            Game result = await context.GetFirstOrDefaultAsync(query);

            if (result is null)
            {
                return new() { Errors = ["game does not exist"] };
            }

            return result.ToGamesResponseDto();
        }

        public async Task<DefaultResponse> AddRiderEvents(RiderEventsRequestDto eventRequest)
        {
            List<string> allowedIndividualResults = ["0", "1", "2", "3", "-", "u", "w"];

            IQueryable<Game> gameQuery = context.Game.Where(x => x.Id.Equals(eventRequest.GameId)).AsQueryable().AsNoTracking();
            Game game = await context.GetFirstOrDefaultAsync(gameQuery);
            if (game is null || game.Inserted)
            {
                return new DefaultResponse() { Errors = ["Bad game id"] };
            }

            if (!new List<string>() { "Home", "Away" }.Contains(eventRequest.HomeAway))
            {
                return new DefaultResponse() { Errors = ["Bad >HOME AWAY< value"] };
            }

            if (eventRequest.HomeAway.Equals("Home") && (eventRequest.GameRiderNr < 9 || eventRequest.GameRiderNr > 16))
            {
                return new DefaultResponse() { Errors = ["Bad >HOME< starting number"] };
            }

            if (eventRequest.HomeAway.Equals("Away") && (eventRequest.GameRiderNr < 1 || eventRequest.GameRiderNr > 8))
            {
                return new DefaultResponse() { Errors = ["Bad >AWAY< starting number"] };
            }

            List<string> individualResults = [];


            try
            {
                individualResults = [..eventRequest.GameRiderResult.Split(',')];
                if (individualResults.Any(x => !allowedIndividualResults.Contains(x)))
                {
                    return new DefaultResponse() { Errors = ["Bad result"] };
                }
            }
            catch (Exception ex)
            {
                return new DefaultResponse() { Errors = [ex.Message] };
            }

            (int, int)[] _1 = [(1, 1), (7, 1), (11, 2), (13, 2)];
            (int, int)[] _2 = [(1, 3), (4, 2), (7, 3), (11, 4)];
            (int, int)[] _3 = [(2, 2), (6, 2), (10, 1), (12, 1)];
            (int, int)[] _4 = [(2, 4), (6, 4), (8, 1), (10, 3)];
            (int, int)[] _5 = [(3, 1), (5, 1), (9, 2), (13, 4)];
            (int, int)[] _6 = [(3, 3), (5, 3), (9, 4), (12, 3)];
            (int, int)[] _7 = [(4, 4), (8, 3)];
            (int, int)[] _8 = [];
            (int, int)[] _9 = [(1, 2), (6, 1), (9, 1), (12, 2)];
            (int, int)[] _10 = [(1, 4), (4, 1), (6, 3), (9, 3)];
            (int, int)[] _11 = [(2, 1), (5, 2), (11, 1), (13, 1)];
            (int, int)[] _12 = [(2, 3), (5, 4), (8, 2), (11, 3)];
            (int, int)[] _13 = [(3, 2), (7, 2), (10, 2), (12, 4)];
            (int, int)[] _14 = [(3, 4), (7, 4), (10, 4), (13, 3)];
            (int, int)[] _15 = [(4, 3), (8, 4)];
            (int, int)[] _16 = [];

            List<(int, int)[]> season13Heats = [_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16];

            List<Event> events = [];
            for (int i = 0; i < individualResults.Count; i++)
            {
                Event riderEvent = new()
                {
                    GameId = eventRequest.GameId,
                    RiderId = eventRequest.RiderId,
                    RiderGameNumber = eventRequest.GameRiderNr,
                    RiderHeatNumber = 99,
                    RiderRowNumber = 0,
                    EventResult = individualResults[i],
                    HomeAway = eventRequest.HomeAway,
                };
                if (i < season13Heats[eventRequest.GameRiderNr - 1].Length)
                {
                    riderEvent.RiderHeatNumber = season13Heats[eventRequest.GameRiderNr - 1][i].Item1;
                    riderEvent.RiderRowNumber = season13Heats[eventRequest.GameRiderNr - 1][i].Item2;
                }
                events.Add(riderEvent);
            }
            
            try
            {
                context.AddRange(events);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new DefaultResponse() { Errors = [ex.Message] };
            }

            return new DefaultResponse() { Success = true };
        }

        public async Task<List<EventResponseDto>> GameEvents(Guid gameId, string homeAway)
        {
            var game = await GetGameById(gameId);
            if(!game.Success)
            {
                throw new Exception("wrong game id");
            }

            List<Event> events = [];
            try
            {
                IQueryable<Event> query = context.Events.Where(x => x.GameId == gameId && x.HomeAway.Equals(homeAway)).AsQueryable().AsNoTracking();
                events = await context.GetEntitiesToListAsync(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            List<EventResponseDto> response = [.. events.Select(x => x.ToEventResponseDto())];

            return response;
        }

        public async Task<GameRiderEventsResponseDto> GameRiderEvents(Guid gameId, string homeAway)
        {
            var game = await GetGameById(gameId);
            if (!game.Success)
            {
                throw new Exception("wrong game id");
            }

            List<VRiderEvent> gameRiderEvents = [];
            try
            {
                IQueryable<VRiderEvent> query = context.V_Rider_Events.Where(x => x.GameId == gameId && x.HomeAway.Equals(homeAway)).OrderBy(x => x.RiderGameNumber).AsQueryable().AsNoTracking();
                gameRiderEvents = await context.GetEntitiesToListAsync(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            GameRiderEventsResponseDto response = new() { GameRiderEvents = gameRiderEvents, Success = true }; 

            return response;
        }

        public async Task<DefaultResponse> DeleteGameRiderEvents(Guid gameId, Guid riderId)
        {
            var game = await GetGameById(gameId);
            if (!game.Success)
            {
                throw new Exception("wrong game id");
            }

            try
            {
                IQueryable<Event> query = context.Events.Where(x => x.GameId == gameId && x.RiderId == riderId).AsQueryable().AsNoTracking();
                context.Events.RemoveRange(query);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new DefaultResponse() { Errors = [ex.Message] };
            }
   

            return new DefaultResponse() { Success = true };
        }

        public async Task<List<EventWithRiderResponseDto>> GameEventsWithRider(Guid gameId)
        {
            var ridersHome = await GameEvents(gameId, "Home");
            var ridersAway = await GameEvents(gameId, "Away");
            var ridersId = ridersHome.Select(x => x.RiderId).Concat(ridersAway.Select(x => x.RiderId)).Distinct().ToList();
            var client = httpClientFactory.CreateClient("figuraHttp");
            var payload = JsonConvert.SerializeObject(ridersId);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/rider/gameRiders", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var gameRiderResponseDtos = JsonConvert.DeserializeObject<List<RiderResponseDto>>(responseString);
            var gameEvents = ridersHome.Concat(ridersAway);
            List<EventWithRiderResponseDto> result = [..gameEvents
                .Select(x => new EventWithRiderResponseDto {EventResponseDto = x, RiderResponseDto = gameRiderResponseDtos!
                .First(y => y.Id.Equals(x.RiderId))})
                .OrderBy(x => x.EventResponseDto!.RiderHeatNumber).ThenBy(x => x.EventResponseDto!.RiderRowNumber)];
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
        public Task<List<GamesResponseDto>> GetGamesBySeasonId(Guid id);
        public Task<GamesResponseDto> GetGameById(Guid id);
        public Task<DefaultResponse> AddRiderEvents(RiderEventsRequestDto eventRequest);
        public Task<List<EventResponseDto>> GameEvents(Guid gameId, string homeAway);
        public Task<GameRiderEventsResponseDto> GameRiderEvents(Guid gameId, string homeAway);
        public Task<DefaultResponse> DeleteGameRiderEvents(Guid gameId, Guid riderId);
        public Task<List<EventWithRiderResponseDto>> GameEventsWithRider(Guid gameId); 
    }
}
