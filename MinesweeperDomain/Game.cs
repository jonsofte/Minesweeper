using Minesweeper.MinefieldCreationStrategy;
using Minesweeper.Tools;
using System.Collections.Generic;

namespace Minesweeper
{   
   public class Game
   {
      private readonly IMinefieldCreationStrategy _minefieldCreationStrategy;
      private MineField minefield;
      private DisplayField display;
      public GameStatus GameStatus { get; private set; } 
      public int NumberOfMoves { get; private set; }
      public int NumberOfFieldsExplored => display.NumberOfFieldsExplored();
      public int NumberOfFlagsUsed => display.NumberOfFlagsUsed();
      public int NumberOfFields => display.NumberOfFields();
      public int FieldWidth => minefield.Width;
      public int FieldHeight => minefield.Height;
      public int NumberOfMines => minefield.NumberOfMines;
      public Display[,] Display => display.DisplayGrid;
      public List<int> GetDisplayAsList() => display.GetDisplayFieldsAsList();

      public Game(IMinefieldCreationStrategy minefieldCreationStrategy)
      {
         _minefieldCreationStrategy = minefieldCreationStrategy;
         GameStatus = GameStatus.Uninitialized;
      }

      public Result StartNewGame(GameConfiguration configuration)
      {
         if (GameStatus == GameStatus.Active) return Result.Fail($"Can't start game. Status is {GameStatus}");
         minefield = new MineField(configuration.Width,configuration.Height, configuration.NumberOfMines, _minefieldCreationStrategy);
         display = new DisplayField(minefield);
         GameStatus = GameStatus.Active;
         NumberOfMoves = 0;
         return Result.Ok();
      }

      public Result<Display> Explore(int x, int y)
      {
         if (x < 0 || y < 0 || x >= FieldWidth || y >= FieldHeight) return Result.Fail<Display>("Invalid input");
         if (GameStatus != GameStatus.Active) 
            return Result.Fail<Display>($"Can't explore field. Game status is {GameStatus}");
         if (display.DisplayGrid[x, y] != Minesweeper.Display.Hidden) 
            return Result.Fail<Display>($"Can't explore field. Field is {display.DisplayGrid[x, y]}");

         Display result = display.Explore(x, y);
         NumberOfMoves++;
         
         if (result == Minesweeper.Display.Explosion)
         {
            display.RevealAllMines();
            GameStatus = GameStatus.EndedFailed;
         }
         else EndGameIfCompleted();

         return Result.Ok<Display>(result);
      }

      private void EndGameIfCompleted()
      {
         if (display.AllMinesFoundOrFlagged() || display.AllMinesFlagged())
         {
            display.RevealNonFlaggedMinesAndEmptyFields();
            GameStatus = GameStatus.EndedSuccess;
         }
      }

      public Result SetFlag(int x, int y)
      {
         if (x < 0 || y < 0 || x >= FieldWidth || y >= FieldHeight) return Result.Fail("Invalid input");
         if (GameStatus != GameStatus.Active)
            return Result.Fail($"Can't set flag. Game status is {GameStatus}");
         if (display.DisplayGrid[x, y] != Minesweeper.Display.Hidden)
            return Result.Fail($"Can't set flag. Field is {display.DisplayGrid[x, y]}");

         display.SetFlag(x, y);
         EndGameIfCompleted();

         return Result.Ok();
      }

      public Result UnSetFlag(int x, int y)
      {
         if (x < 0 || y < 0 || x >= FieldWidth || y >= FieldHeight) return Result.Fail("Invalid input");
         if (GameStatus != GameStatus.Active)
            return Result.Fail($"Can't unset flag. Game status is {GameStatus}");
         if (display.DisplayGrid[x, y] != Minesweeper.Display.Flagged)
            return Result.Fail($"Can't unset flag. Field is {display.DisplayGrid[x, y]}");

         display.UnSetFlag(x, y);
         return Result.Ok();
      }
      public Result AbortGame()
      {
         if (GameStatus != GameStatus.Active) return Result.Fail($"Can't abort game. Status is {GameStatus}");
         GameStatus = GameStatus.Aborted;
         return Result.Ok(); 
      }
   }
}
