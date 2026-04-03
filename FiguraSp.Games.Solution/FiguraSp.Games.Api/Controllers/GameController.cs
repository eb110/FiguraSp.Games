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
    }
}
