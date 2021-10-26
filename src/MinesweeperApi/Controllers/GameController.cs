using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using MinesweeperApi.Models;
using Minesweeper.Api.Commands;
using Minesweeper.Api.Queries;
using System;

namespace Minesweeper.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class GameController : ControllerBase
   {
      private readonly IMediator _mediator;
      public GameController(IMediator mediator)
      {
         _mediator = mediator;
      }

      [HttpPost("NewGame")]
      public async Task<ActionResult<MinesweeperApi.Models.Minesweeper>> PostStartNewGame(NewGameRequest gameRequest)
      {
         var createNewGameCommand = new CreateNewGameCommand()
         {
            Width = gameRequest.Width,
            Height = gameRequest.Height,
            NumberOfMines = gameRequest.NumberOfMines
         };

         var createGameResponse = await _mediator.Send(createNewGameCommand);

         if (createGameResponse.Success)
         {
            var getGameQuery = new GetGameQuery() { GameID = createGameResponse.Value };
            var getGameResponse = await _mediator.Send(getGameQuery);
            return getGameResponse.Success ?
               new ActionResult<MinesweeperApi.Models.Minesweeper>(getGameResponse.Value) :
               new BadRequestObjectResult(getGameResponse.Error);
         }
         else
         {
            return new BadRequestObjectResult(createGameResponse.Error);
         }
      }

      [HttpGet]
      public async Task<ActionResult<IReadOnlyList<string>>> GetAllGames()
      {
         var getCurrentGames = new GetCurrentGamesQuery();
         var getCurrentGamesResponse = await _mediator.Send(getCurrentGames);
         return new ActionResult<IReadOnlyList<string>>(getCurrentGamesResponse.Select(x => x.GameID).ToList());
      }

      [HttpGet("{gameID}")]
      public async Task<ActionResult<MinesweeperApi.Models.Minesweeper>> GetGame(string gameID)
      {
         var getGameQuery = new GetGameQuery() { GameID = gameID };
         var getGameResponse = await _mediator.Send(getGameQuery);
         return getGameResponse.Success ?
            new ActionResult<MinesweeperApi.Models.Minesweeper>(getGameResponse.Value) :
            new BadRequestObjectResult(getGameResponse.Error);
      }


      [HttpPost("{gameID}")]
      public async Task<ActionResult<MinesweeperApi.Models.Minesweeper>> PostGame(string gameID, GameActionRequest action)
      {
         static Api.Commands.Action getAction(MinesweeperApi.Models.Action action) => action switch
         {
            MinesweeperApi.Models.Action.Explore => Api.Commands.Action.Explore,
            MinesweeperApi.Models.Action.Flag => Api.Commands.Action.Flag,
            MinesweeperApi.Models.Action.Quit => Api.Commands.Action.Quit,
            MinesweeperApi.Models.Action.Unflag => Api.Commands.Action.Unflag,
            _ => throw new ArgumentOutOfRangeException(nameof(action), $"Invalid Action: {action}")
         };

         var executeActionCommand = new ExecuteActionCommand()
         {
            GameId = gameID,
            ActionType = getAction(action.ActionType),
            X = action.X,
            Y = action.Y
         };

         var executeActionResponse = await _mediator.Send(executeActionCommand);

         if (executeActionResponse.Success)
         {
            var getGameQuery = new GetGameQuery() { GameID = gameID };
            var getGameResponse = await _mediator.Send(getGameQuery);
            return getGameResponse.Success ?
               new ActionResult<MinesweeperApi.Models.Minesweeper>(getGameResponse.Value) :
               new BadRequestObjectResult(getGameResponse.Error);
         }
         else
         {
            return new BadRequestObjectResult(executeActionResponse.Error);
         }
      }
   }
}
