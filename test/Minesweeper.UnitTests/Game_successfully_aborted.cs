using Xunit;
using Minesweeper;
using Microsoft.Extensions.DependencyInjection;
using Minesweeper.UnitTests;

namespace MinesweeperTest
{
   public class Game_successfully_aborted
   {
      private readonly GameFactory _factory;

      public Game_successfully_aborted()
      {
         ServiceCollection services = new ServiceCollection();
         services.AddMinefieldTestGame();
         _factory = (GameFactory)ActivatorUtilities.CreateInstance(services.BuildServiceProvider(), typeof(GameFactory));
      }
      [Fact]
      public void Abort_running_game_ends_game_and_set_game_status_to_aborted()
      {
         Game minesweeper = _factory.CreateNewGame();
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         minesweeper.AbortGame();
         Assert.Equal(GameStatus.Aborted, minesweeper.GameStatus);
      }
   }
}
