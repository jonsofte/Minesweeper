using Xunit;
using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;

namespace MinesweeperTest
{
   public class Game_successfully_ended_with_explosion
   {

      [Fact]
      public void Exploring_field_with_bomb_causes_explosion()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         var result = minesweeper.Explore(0,3);
         Assert.Equal(Display.Explosion, result.Value);
      }

      [Fact]
      public void Explosion_causes_game_to_end_with_status_failed()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         minesweeper.Explore(0, 3);
         Assert.Equal(GameStatus.EndedFailed, minesweeper.GameStatus);
      }
   }
}
