
using Xunit;
using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;

namespace MinesweeperTest
{
   public class Game_successfully_completed_with_flags
   {
      [Fact]
      public void Display_field_is_updated_when_flag_is_set()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         minesweeper.SetFlag(0, 4);
         Assert.Equal(Display.Flagged, minesweeper.Display[0,4]);
      }

      [Fact]
      public void Display_field_is_updated_when_flag_is_removed()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         minesweeper.SetFlag(0, 4);
         minesweeper.UnSetFlag(0, 4);
         Assert.Equal(Display.Hidden, minesweeper.Display[0, 4]);
      }

      [Fact]
      public void Game_is_completed_successfully_when_all_flags_are_set_correctly()
      {
         Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         GameConfiguration configuration = new GameConfiguration(10, 10, numberOfMines: 5);
         minesweeper.StartNewGame(configuration);
         minesweeper.SetFlag(0, 0);
         minesweeper.SetFlag(0, 1);
         minesweeper.SetFlag(0, 2);
         minesweeper.SetFlag(0, 3);
         minesweeper.SetFlag(0, 4);
         Assert.Equal(GameStatus.EndedSuccess, minesweeper.GameStatus);
         Assert.Equal(5, minesweeper.NumberOfFlagsUsed);
         Assert.Equal(95, minesweeper.NumberOfFieldsExplored);
      }
   }
}
