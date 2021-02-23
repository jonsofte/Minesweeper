
using Xunit;
using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;

namespace MinesweeperTest
{
    public class Game_successfully_completed_without_flags
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

      [Theory]
      [InlineData(1, 0, Display.Two)]
      [InlineData(1, 1, Display.Three)]
      [InlineData(1, 5, Display.One)]
      [InlineData(5, 5, Display.Empty)]
      public void Explore_fields_gives_correct_display_value(int x, int y, Display expected)
      {
         minesweeper.StartNewGame(configuration);
         var result = minesweeper.Explore(x, y);
         Assert.Equal(expected, result.Value);
      }

      [Fact]
       public void Correct_number_of_moves()
      {
         minesweeper.StartNewGame(configuration);
         minesweeper.Explore(1,0);
         minesweeper.Explore(1,1);
         minesweeper.Explore(1,5);
         minesweeper.Explore(5,5);
         Assert.Equal(4, minesweeper.NumberOfMoves);
      }

      [Fact]
      public void Game_is_ended_successfully()
      {
         minesweeper.StartNewGame(configuration);
         minesweeper.Explore(1, 0);
         minesweeper.Explore(1, 1);
         minesweeper.Explore(1, 5);
         minesweeper.Explore(5, 5);
         Assert.Equal(GameStatus.EndedSuccess, minesweeper.GameStatus);
         Assert.Equal(95, minesweeper.NumberOfFieldsExplored);
      }

      [Fact]
      public void Total_number_of_fields_is_correct()
      {
         minesweeper.StartNewGame(configuration);
         Assert.Equal(100, minesweeper.NumberOfFields);
      }
   }
}
