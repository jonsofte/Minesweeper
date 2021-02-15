using Minesweeper;
using System;

namespace MinesweeperConsole
{
   class Program
   {
      static void Main()
      {
         Random rand = new Random();
         Game minesweeper = new Game();

         int width = 30;
         int height = 20;
         int numberOfMines = 30;

         minesweeper.StartNewGame(width, height, numberOfMines);
         DisplayFieldConsole display = new DisplayFieldConsole(minesweeper.Display);

         display.PrintDisplay();

         while (minesweeper.gameStatus == GameStatus.Active)
         {
            Console.ReadLine();
            var (x, y) = (rand.Next(width-1), rand.Next(height-1));
            minesweeper.Explore(x,y);

            display.PrintDisplay();
            Console.WriteLine($"Explored: {x+1},{y+1}");
            Console.WriteLine($"Status {minesweeper.gameStatus}");
            Console.WriteLine("-----------------------------------------------------");
         }
      }
   }
}
