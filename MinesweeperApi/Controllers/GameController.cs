using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MinesweeperApi.Models;
using MinesweeperApi.Service;

namespace Minesweeper.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class GameController : ControllerBase
   {
      private readonly GameService _gameService;
      public GameController(GameService service) => _gameService = service;

      [HttpPost("NewGame")]
      public ActionResult<MinesweeperApi.Models.Minesweeper> PostStartNewGame(NewGameRequest gameRequest)
      {
         var createGameResult = _gameService.CreateNewGame(gameRequest.Width, gameRequest.Height, gameRequest.NumberOfMines);
         return createGameResult.Success ?
            new ActionResult<MinesweeperApi.Models.Minesweeper>(_gameService.GetGame(createGameResult.Value).Value) :
            new BadRequestObjectResult(createGameResult.Error);
      }

      [HttpGet]
      public ActionResult<List<string>> GetAllGames()
      {
         var currentGames = _gameService.GetCurrentGames().Select(x => x.GameID).ToList();
         return new ActionResult<List<string>>(currentGames);
      }

      [HttpGet("{gameID}")]
      public ActionResult<MinesweeperApi.Models.Minesweeper> GetGame(string gameID)
      {
         var result = _gameService.GetGame(gameID);
         return result.Success ?
            new ActionResult<MinesweeperApi.Models.Minesweeper>(result.Value) :
            new BadRequestObjectResult(result.Error);
      }


      [HttpPost("{gameID}")]
      public ActionResult<MinesweeperApi.Models.Minesweeper> PostGame(string gameID, GameActionRequest action)
      {
         var result = _gameService.ExecuteAction(gameID, action);
         return result.Success ?
            new ActionResult<MinesweeperApi.Models.Minesweeper>(_gameService.GetGame(gameID).Value) :
            new BadRequestObjectResult(result.Error);
      }
   }
}
