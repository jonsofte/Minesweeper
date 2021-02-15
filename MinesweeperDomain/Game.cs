
using Minesweeper.MinefieldCreationStrategy;

namespace Minesweeper
{   
   public class Game
   {
      public GameStatus gameStatus = GameStatus.Uninitialized;
      private MineField minefield;
      private DisplayField display;
      public Display[,] Display => display.Display;

      public void StartNewGame(int width, int height, int numberOfMines)
      {
         minefield = new MineField(width, height, numberOfMines, new RandomMinefieldCreationStrategy());
         display = new DisplayField(minefield);
         gameStatus = GameStatus.Active;
      }

      public void Explore(int x, int y)
      {
         Display result = display.Explore(x, y);
         if (result == Minesweeper.Display.Explosion) gameStatus = GameStatus.EndedFailed;
      }
   }
}
