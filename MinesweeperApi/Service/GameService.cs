using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minesweeper;
using Minesweeper.Tools;
using MinesweeperApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinesweeperApi.Service
{
   public class GameService
   {
      private readonly ILogger<GameService> _logger;
      private readonly GameSettingsOption _gameSettings;
      private readonly Dictionary<string, GameSession> _games;
      private readonly GameFactory _gameFactory;
      public GameService(ILogger<GameService> logger, IConfiguration configuration)
      {
         _logger = logger;
         _gameSettings = configuration.GetSection(GameSettingsOption.GameSettings).Get<GameSettingsOption>();

         _gameFactory = new GameFactory();
         _games = new Dictionary<string, GameSession>();
         _logger.LogInformation("Initializing Gameservice");
      }

      public Result<string> CreateNewGame(int width, int height, int numberOfMines)
      {
         RemoveExpiredGames();
         var gameSession = new GameSession(game: _gameFactory.CreateNewGame());
         gameSession.StartGame(width, height, numberOfMines);
         _games.Add(gameSession.Guid.ToString(), gameSession);

         _logger.LogInformation($"Starting new game: ({width}x{height} {numberOfMines} mines)");

         return Result.Ok<string>(gameSession.Guid.ToString());
      }

      public List<Models.Minesweeper> GetCurrentGames() => _games.Select(x => x.Value).Select(MapToModel).ToList();

      public Result<Models.Minesweeper> GetGame(string value) => _games.ContainsKey(value) ?
         Result.Ok<Models.Minesweeper>(MapToModel(_games[value])) :
         Result.Fail<Models.Minesweeper>("Invalid Game ID");

      public Result ExecuteAction(string gameid, GameActionRequest action)
      {
         if (!_games.ContainsKey(gameid)) return Result.Fail("Invalid Game ID");
         var gameSession = _games[gameid];
         switch (action.ActionType)
         {
            case Models.Action.Explore:
               var result = gameSession.Explore(action.X, action.Y);
               if (result.Failure) return Result.Fail(result.Error);
               break;
            case Models.Action.Flag:
               result = gameSession.SetFlag(action.X, action.Y);
               if (result.Failure) return Result.Fail(result.Error);
               break;
            case Models.Action.Unflag:
               result = gameSession.UnSetFlag(action.X, action.Y);
               if (result.Failure) return Result.Fail(result.Error);
               break;
            case Models.Action.Quit:
               result = gameSession.AbortGame();
               if (result.Failure) return Result.Fail(result.Error);
               break;
            default:
               return Result.Fail("Invalid action type");
         }
         return Result.Ok();
      }

      private Models.Minesweeper MapToModel(GameSession session) => new Models.Minesweeper()
      {
         GameID = session.Guid.ToString(),
         GameStartedTime = session.GameStartedTime,
         GameStatus = session.Game.GameStatus.ToString(),
         NumberOfMoves = session.Game.NumberOfMoves,
         NumberOfFlagsUsed = session.Game.NumberOfFlagsUsed,
         NumberOfFieldsExplored = session.Game.NumberOfFieldsExplored,
         Minefield = new Minefield()
         {
            NumberOfFields = session.Game.NumberOfFields,
            Width = session.Game.FieldWidth,
            Height = session.Game.FieldHeight,
            GridData = session.Game.GetDisplayAsList(),
            NumberOfMines = session.Game.NumberOfMines,
            FieldTypeValues = GetDisplayEnumValues()
         }
      };

      private static Dictionary<int, string> GetDisplayEnumValues() => 
         Enum.GetValues(typeof(Display)).Cast<Display>().ToDictionary(x => (int)x, x => x.ToString());

      private void RemoveExpiredGames()
      {
         var gamesToRemove = _games.Where(x =>
            x.Value.Game.GameStatus != GameStatus.Active && 
            x.Value.GameStartedTime < DateTime.Now.AddHours(-1 * _gameSettings.MinutesBeforeDeletingOldGames))
            .Select(s => s.Key).ToList();

         if (gamesToRemove.Count > 0) 
         { 
            _logger.LogInformation($"Removing {gamesToRemove.Count} expired games");
            gamesToRemove.ForEach(x => _games.Remove(x));
         }
      }
   }
}
