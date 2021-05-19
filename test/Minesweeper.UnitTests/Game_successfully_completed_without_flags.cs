
using Xunit;
using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;

namespace MinesweeperTest
{
    public class Game_successfully_completed_without_flags
   {
      [Theory]
      [InlineData(1, 0, Display.Two)]
      [InlineData(1, 1, Display.Three)]
      [InlineData(1, 5, Display.One)]
      [InlineData(5, 5, Display.Empty)]
      public void Explore_field_gives_correct_display_number_value(int x, int y, Display displayValueExpected)
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         var result = minesweeper.Explore(x, y);
         Assert.Equal(displayValueExpected, result.Value);
      }

      [Fact]
       public void NumberOfMoves_value_is_updated_when_exploring()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         minesweeper.Explore(1,0);
         minesweeper.Explore(1,1);
         minesweeper.Explore(1,5);
         minesweeper.Explore(5,5);
         Assert.Equal(4, minesweeper.NumberOfMoves);
      }

      [Fact]
      public void Game_is_completed_successfully_when_all_none_mine_fields_are_explored()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         minesweeper.Explore(1, 0);
         minesweeper.Explore(1, 1);
         minesweeper.Explore(1, 5);
         minesweeper.Explore(5, 5);
         Assert.Equal(GameStatus.EndedSuccess, minesweeper.GameStatus);
         Assert.Equal(95, minesweeper.NumberOfFieldsExplored);
      }

      [Fact]
      public void NumberOfFields_value_matches_game_configuration_setup()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         Assert.Equal(100, minesweeper.NumberOfFields);
      }
   }
}
