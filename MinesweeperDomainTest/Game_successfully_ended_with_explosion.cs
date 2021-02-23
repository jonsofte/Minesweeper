using Xunit;
using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;

namespace MinesweeperTest
{
   public class Game_successfully_ended_with_explosion
   {
      readonly Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
      readonly GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);

      [Fact]
      public void Game_is_unintialized()
      {
         Assert.Equal(GameStatus.Uninitialized, minesweeper.GameStatus);
      }

      [Fact]
      public void Staring_new_game_gives_status_active()
      {
         minesweeper.StartNewGame(configuration);
         Assert.Equal(GameStatus.Active, minesweeper.GameStatus);
      }

      [Fact]
      public void Exploring_field_with_bomb_causes_explosion()
      {
         minesweeper.StartNewGame(configuration);
         var result = minesweeper.Explore(0,3);
         Assert.Equal(Display.Explosion, result.Value);
      }

      [Fact]
      public void Explosion_causes_game_to_end()
      {
         minesweeper.StartNewGame(configuration);
         minesweeper.Explore(0, 3);
         Assert.Equal(GameStatus.EndedFailed, minesweeper.GameStatus);
      }
   }
}
