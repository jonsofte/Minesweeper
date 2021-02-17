﻿using Xunit;
using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;

namespace MinesweeperTest
{
   public class Game_successfully_aborted
   {
      readonly Game minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
      readonly GameConfiguration configuration = new GameConfiguration() { Height = 10, Width = 10, NumberOfMines = 5 };

      [Fact]
      public void Game_is_unintialized()
      {
         Assert.Equal(GameStatus.Uninitialized, minesweeper.GameStatus);
      }

      [Fact]
      public void Staring_new_game_gives_status_active()
      {
         minesweeper.StartNewGame(configuration.Width, configuration.Height, configuration.NumberOfMines);
         Assert.Equal(GameStatus.Active, minesweeper.GameStatus);
      }

      [Fact]
      public void Ending_game_with_abort()
      {
         minesweeper.StartNewGame(configuration.Width, configuration.Height, configuration.NumberOfMines);
         minesweeper.AbortGame();
         Assert.Equal(GameStatus.Aborted, minesweeper.GameStatus);
      }
   }
}
