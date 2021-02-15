using Mineweeper;
using System;

namespace MinesweeperConsole
{
   class Program
   {
      static void Main(string[] args)
      {
         Random rand = new Random();
         Game minesweeper = new Game();
         int width = 40;
         int height = 20;
         minesweeper.StartNewGame(width, height, numberOfMines: 15);
         DisplayFieldConsole display = new DisplayFieldConsole(minesweeper.Display);
         display.printDisplay();

         while (minesweeper.gameStatus == GameStatus.Active)
         {
            Console.ReadLine();
            var point = (x: rand.Next(width-1), y: rand.Next(height-1));
            minesweeper.Explore(point.x,point.y);

            display.printDisplay();
            Console.WriteLine($"Explored: {point.x},{point.y}");
            Console.WriteLine($"Status {minesweeper.gameStatus}");
            Console.WriteLine("-----------------------------------------------------");
         }
      }
   }
}
