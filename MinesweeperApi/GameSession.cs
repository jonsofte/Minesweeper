using Minesweeper;
using Minesweeper.Tools;
using MinesweeperApi.Models;
using System;
using System.Collections.Generic;

namespace MinesweeperApi
{
   public class GameSession
   {
      public Guid Guid { get; private set; }
      public DateTimeOffset GameStartedTime { get; private set; }
      public Game Game { get; private set; }
      public List<(DateTimeOffset, string)> GameMoves { get; private set; }
      public bool IsActive => Game.GameStatus == GameStatus.Active;

      public GameSession(Game game)
      {
         Guid = Guid.NewGuid();
         GameStartedTime = DateTimeOffset.Now;
         Game = game;
         GameMoves = new List<(DateTimeOffset, string)>();
      }

      public Result Explore(int x, int y)
      {
         var result = Game.Explore(x, y);
         if (result.Success) AddEventToGameLog($"Explored field {x},{y}");
         return result;
      }
      public Result SetFlag(int x, int y)
      {
         var result = Game.SetFlag(x,y);
         if (result.Success) AddEventToGameLog($"Set flag at field {x},{y}");
         return result;
      }
      public Result UnSetFlag(int x, int y)
      {
         var result = Game.UnSetFlag(x, y);
         if (result.Success) AddEventToGameLog($"Unset flag at field {x},{y}");
         return result;
      }
      public Result AbortGame()
      {
         var result = Game.AbortGame();
         if (result.Success) AddEventToGameLog($"Game Aborted");
         return result;
      }
      
      public void StartGame(int width, int height, int numberOfMines)
      {
         GameConfiguration configuration = new GameConfiguration(width, height, numberOfMines);
         Game.StartNewGame(configuration);
         AddEventToGameLog($"Started new game with configuration: w:{width} h:{height} m:{numberOfMines}");
      }

      private void AddEventToGameLog(string gameEvent)
      {
         GameMoves.Add((DateTimeOffset.Now, gameEvent));
      }

      public Models.Minesweeper MapToModel() => new Models.Minesweeper()
      {
         GameID = Guid.ToString(),
         GameStartedTime = GameStartedTime,
         GameStatus = Game.GameStatus.ToString(),
         NumberOfMoves = Game.NumberOfMoves,
         NumberOfFlagsUsed = Game.NumberOfFlagsUsed,
         NumberOfFieldsExplored = Game.NumberOfFieldsExplored,
         Minefield = new Minefield()
         {
            NumberOfFields = Game.NumberOfFields,
            Width = Game.FieldWidth,
            Height = Game.FieldHeight,
            GridData = Game.GetDisplayAsList(),
            NumberOfMines = Game.NumberOfMines,
            FieldTypeValues = Game.GetDisplayEnumValues()
         }
      };
   }
}
