using Xunit;
using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;

namespace MinesweeperTest
{
   public class Game_Initializes_completly  
   {
      [Fact]
      public void Unconfigured_game_is_started_as_uninitialized()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         Assert.Equal(GameStatus.Uninitialized, minesweeper.GameStatus);
      }

      [Fact]
      public void Staring_new_game_gives_status_active()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         Assert.Equal(GameStatus.Active, minesweeper.GameStatus);
      }

      [Fact]
      public void Game_width_is_set_when_initialized_new_game()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(10, 20, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         Assert.Equal(10, minesweeper.FieldWidth);
      }

      [Fact]
      public void Game_height_is_set_when_initialized_new_game()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(20, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         Assert.Equal(10, minesweeper.FieldHeight);
      }

      [Fact]
      public void NumberOfMines_is_set_when_initialized_new_game()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(20, 30, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         Assert.Equal(5, minesweeper.NumberOfMines);
      }
   }
}
