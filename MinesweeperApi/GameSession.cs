using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;
using Minesweeper.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MinesweeperApi
{
   public class GameSession
   {
      public Guid Guid { get; private set; }
      public DateTimeOffset GameStartedTime { get; private set; }
      public Game Game { get; private set; }
      public List<(DateTimeOffset, string)> GameMoves { get; private set; }

      public GameSession()
      {
         Guid = System.Guid.NewGuid();
         GameStartedTime = DateTimeOffset.Now;
         Game = new Game(new RandomMinefieldCreationStrategy());
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
   }
}
