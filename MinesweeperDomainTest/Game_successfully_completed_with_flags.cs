
using Xunit;
using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;

namespace MinesweeperTest
{
   public class Game_successfully_completed_with_flags
   {
      readonly Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
      readonly GameConfiguration configuration = new GameConfiguration() { Height = 10, Width = 10, NumberOfMines = 5 };

      [Fact]
      public void Setting_of_flag_is_correct()
      {
         minesweeper.StartNewGame(configuration.Width, configuration.Height, configuration.NumberOfMines);
         minesweeper.SetFlag(0, 4);
         Assert.Equal(Display.Flagged, minesweeper.Display[0,4]);
      }

      [Fact]
      public void Unsetting_of_flag_is_correct()
      {
         minesweeper.StartNewGame(configuration.Width, configuration.Height, configuration.NumberOfMines);
         minesweeper.SetFlag(0, 4);
         minesweeper.UnSetFlag(0, 4);
         Assert.Equal(Display.Hidden, minesweeper.Display[0, 4]);
      }

      [Fact]
      public void Game_is_ended_successfully_when_all_flags_are_set()
      {
         minesweeper.StartNewGame(configuration.Width, configuration.Height, configuration.NumberOfMines);
         minesweeper.SetFlag(0, 0);
         minesweeper.SetFlag(0, 1);
         minesweeper.SetFlag(0, 2);
         minesweeper.SetFlag(0, 3);
         minesweeper.SetFlag(0, 4);
         Assert.Equal(GameStatus.EndedSuccess, minesweeper.GameStatus);
         Assert.Equal(0, minesweeper.NumberOfFieldsExplored());
      }
   }
}
