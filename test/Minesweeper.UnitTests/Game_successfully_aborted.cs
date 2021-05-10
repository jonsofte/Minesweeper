using Xunit;
using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;

namespace MinesweeperTest
{
   public class Game_successfully_aborted
   {

      [Fact]
      public void Abort_running_game_ends_game_and_set_game_status_to_aborted()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         minesweeper.AbortGame();
         Assert.Equal(GameStatus.Aborted, minesweeper.GameStatus);
      }
   }
}
