using Xunit;
using Minesweeper;
using Microsoft.Extensions.DependencyInjection;
using Minesweeper.UnitTests;

namespace MinesweeperTest
{
   public class Game_Initializes_Completely
   {
      private readonly GameFactory _factory;

      public Game_Initializes_Completely()
      {
         ServiceCollection services = new ServiceCollection();
         services.AddMinefieldTestGame();
         _factory = (GameFactory)ActivatorUtilities.CreateInstance(services.BuildServiceProvider(), typeof(GameFactory));
      }

      [Fact]
      public void Unconfigured_game_is_started_as_uninitialized()
      {
         Game minesweeper = _factory.CreateNewGame();
         Assert.Equal(GameStatus.Uninitialized, minesweeper.GameStatus);
      }

      [Fact]
      public void Staring_new_game_gives_status_active()
      {
         Game minesweeper = _factory.CreateNewGame();
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         Assert.Equal(GameStatus.Active, minesweeper.GameStatus);
      }

      [Fact]
      public void Game_width_is_set_when_initialized_new_game()
      {
         Game minesweeper = _factory.CreateNewGame();
         GameConfiguration configuration = new GameConfiguration(10, 20, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         Assert.Equal(10, minesweeper.FieldWidth);
      }

      [Fact]
      public void Game_height_is_set_when_initialized_new_game()
      {
         Game minesweeper = _factory.CreateNewGame();
         GameConfiguration configuration = new GameConfiguration(20, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         Assert.Equal(10, minesweeper.FieldHeight);
      }

      [Fact]
      public void NumberOfMines_is_set_when_initialized_new_game()
      {
         Game minesweeper = _factory.CreateNewGame();
         GameConfiguration configuration = new GameConfiguration(20, 30, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         Assert.Equal(5, minesweeper.NumberOfMines);
      }
   }
}
