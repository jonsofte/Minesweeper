
namespace Mineweeper
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
         display = new DisplayField(minefield, width, height);
         gameStatus = GameStatus.Active;
      }

      public void Explore(int x, int y)
      {
         Display result = display.Explore(x, y);
         if (result == Mineweeper.Display.Explosion) gameStatus = GameStatus.EndedFailed;
      }
   }
}
