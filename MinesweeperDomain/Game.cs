using Minesweeper.MinefieldCreationStrategy;
using System;

namespace Minesweeper
{   
   public class Game
   {
      private readonly IMinefieldCreationStrategy _minefieldCreationStrategy;
      private MineField minefield;
      private DisplayField display;

      public GameStatus GameStatus { get; private set; } 
      public Display[,] Display => display.DisplayGrid;
      public int NumberOfMoves;

      public Game(IMinefieldCreationStrategy minefieldCreationStrategy)
      {
         _minefieldCreationStrategy = minefieldCreationStrategy;
         GameStatus = GameStatus.Uninitialized;
      }

      public void StartNewGame(int width, int height, int numberOfMines)
      {
         minefield = new MineField(width, height, numberOfMines, _minefieldCreationStrategy);
         display = new DisplayField(minefield);
         GameStatus = GameStatus.Active;
         NumberOfMoves = 0;
      }

      public Display Explore(int x, int y)
      {
         Display result = display.Explore(x, y);
         NumberOfMoves++;
         
         if (result == Minesweeper.Display.Explosion) 
            GameStatus = GameStatus.EndedFailed;
         else if (display.AllMinesFoundOrFlagged() || display.AllMinesFlagged()) 
            GameStatus = GameStatus.EndedSuccess;
         return result;
      }

      public void SetFlag(int x, int y)
      {
         display.SetFlag(x, y);
         if (display.AllMinesFoundOrFlagged() || display.AllMinesFlagged())
            GameStatus = GameStatus.EndedSuccess;
      }

      public int NumberOfFieldsExplored => display.NumberOfFieldsExplored();
      public int NumberOfFlagsUsed => display.NumberOfFlagsUsed();
      public int NumberOfFields => display.NumberOfFields();
      public void UnSetFlag(int x, int y) => display.UnSetFlag(x, y);
      public void AbortGame() => GameStatus = GameStatus.Aborted;
   }
}
