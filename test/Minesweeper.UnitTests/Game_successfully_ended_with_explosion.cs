using Xunit;
using Minesweeper;
using Microsoft.Extensions.DependencyInjection;
using Minesweeper.UnitTests;

namespace MinesweeperTest
{
   public class Game_successfully_ended_with_explosion
   {
      private readonly GameFactory _factory;

      public Game_successfully_ended_with_explosion()
      {
         ServiceCollection services = new ServiceCollection();
         services.AddMinefieldTestGame();
         _factory = (GameFactory)ActivatorUtilities.CreateInstance(services.BuildServiceProvider(), typeof(GameFactory));
      }

      [Fact]
      public void Exploring_field_with_bomb_causes_explosion()
      {
         Game minesweeper = _factory.CreateNewGame();
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         var result = minesweeper.Explore(0,3);
         Assert.Equal(Display.Explosion, result.Value);
      }

      [Fact]
      public void Explosion_causes_game_to_end_with_status_failed()
      {
         Game minesweeper = _factory.CreateNewGame();
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         minesweeper.Explore(0, 3);
         Assert.Equal(GameStatus.EndedFailed, minesweeper.GameStatus);
      }
   }
}
