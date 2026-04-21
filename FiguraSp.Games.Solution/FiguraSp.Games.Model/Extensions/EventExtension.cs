using FiguraSp.Games.Model.Entity;
using FiguraSp.Games.Model.Responses;

namespace FiguraSp.Games.Model.Extensions
{
    public static class EventExtension
    {
        public static EventResponseDto ToEventResponseDto(this Event gameEvent)
        {
            EventResponseDto response = new()
            {
                Id = gameEvent.Id,
                GameId = gameEvent.GameId,
                RiderId = gameEvent.RiderId,
                RiderGameNumber = gameEvent.RiderGameNumber,
                RiderHeatNumber = gameEvent.RiderHeatNumber,
                RiderRowNumber = gameEvent.RiderRowNumber,
                EventResult = gameEvent.EventResult,
                HomeAway = gameEvent.HomeAway,
                ChangedFromRiderId = gameEvent.ChangedFromRiderId,
                ChangedToRiderId = gameEvent.ChangedToRiderId,
                Success = true
            };

            return response;
        }
    }
}
