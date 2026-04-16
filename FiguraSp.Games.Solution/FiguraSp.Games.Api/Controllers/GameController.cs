using FiguraSp.Games.Model.Requests;
using FiguraSp.Games.Model.Responses;
using FiguraSp.Games.Service.Services;
using FiguraSp.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FiguraSp.Games.Api.Controllers
{
    [Route("api/[controller]")] // http://localhost:5000/api/game
    [ApiController]
    public class GameController(IGameService gameService) : ControllerBase
    {
        [HttpGet]
        [Route("Seasons")]
        public async Task<ActionResult<List<SeasonResponseDto>>> GetAllSeasons()
        {
            var seasons = await gameService.GetSeasons();
            return Ok(seasons);
        }

        [HttpGet]
        [Route("Levels")]
        public async Task<ActionResult<List<PicklistGameLevelResponseDto>>> GetLevels()
        {
            var response = await gameService.GetLevels();
            return Ok(response);
        }

        [HttpGet]
        [Route("Season")]
        public async Task<ActionResult<SeasonResponseDto>> GetSeasonById(Guid id)
        {
            var response = await gameService.GetSeasonById(id);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost]
        [Route("RiderEvents")]
        public async Task<ActionResult<DefaultResponse>> AddRiderEvents(RiderEventsRequestDto riderEventsRequest)
        {
            var response = await gameService.AddRiderEvents(riderEventsRequest);

            return response;
        }

        [HttpGet]
        [Route("GameEvents")]
        public async Task<ActionResult<List<EventResponseDto>>> GameEvents(Guid gameId, string homeAway)
        {
            var response = await gameService.GameEvents(gameId, homeAway);

            return Ok(response);
        }

        [HttpGet]
        [Route("GameRiderEvents")]
        public async Task<ActionResult<List<GameRiderEventsResponseDto>>> GameRiderEvents(Guid gameId, string homeAway)
        {
            var response = await gameService.GameRiderEvents(gameId, homeAway);

            return Ok(response);
        }

        [HttpGet]
        [Route("GameEventsWithRider")]
        public async Task<ActionResult<List<EventWithRiderResponseDto>>> EventWithRiders(Guid gameId)
        {
            var response = await gameService.GameEventsWithRider(gameId);

            return Ok(response);
        }

        [HttpDelete]
        [Route("RemoveGameRiderEvents")]
        public async Task<ActionResult> RemoveGameRiderEvents(Guid gameId, Guid riderId)
        {
            var response = await gameService.DeleteGameRiderEvents(gameId, riderId);
            if(!response.Success)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpGet]
        [Route("SeasonByYear")]
        public async Task<ActionResult<SeasonResponseDto>> GetSeasonByYear(string year)
        {
            var response = await gameService.GetSeasonByYear(year);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        [Route("SeasonGames")]
        public async Task<ActionResult<List<GamesResponseDto>>> GetSeasonGames(Guid seasonId)
        {
            var response = await gameService.GetGamesBySeasonId(seasonId);
            return Ok(response);
        }

        [HttpPost]
        [Route("Season")]
        public async Task<ActionResult<SeasonResponseDto>> CreateSeason(string year)
        {
            var response = await gameService.AddSeason(year);

            if(response.Success)
            {
                return CreatedAtAction("GetSeasonByYear", new { year }, response);
            }

            return BadRequest(response);
        }

        [HttpPost]
        [Route("Games")]
        public async Task<ActionResult<DefaultResponse>> CreateGames([FromBody] GamesRequestDto gamesRequest)
        {
            var response = await gameService.AddGamesList(gamesRequest);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        public async Task<ActionResult<GamesResponseDto>> GetGame(Guid id)
        {
            var response = await gameService.GetGameById(id);

            return response;
        }
    }
}
