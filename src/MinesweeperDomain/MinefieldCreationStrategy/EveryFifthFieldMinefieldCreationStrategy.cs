using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.MinefieldCreationStrategy
{
   public class EveryFifthFieldMinefieldCreationStrategy : IMinefieldCreationStrategy
   {
      // Arrange mines in pattern:
      // X....X....X..
      // X....X....X..

      public Land[,] CreateMinefield(int width, int height, int numberOfMines)
      {
         Land[,] minefield = new Land[width, height];
         var mines = new HashSet<(int x, int y)>();

         foreach (int y in Enumerable.Range(0, height - 1))
         {
            foreach (int x in Enumerable.Range(0, (width - 1) / 5))
            {
               if (mines.Count < numberOfMines) mines.Add((5*x, y));
            }
         }

         mines.ToList().ForEach(mine => minefield[mine.x, mine.y] = Land.Mine);
         return minefield;
      }
   }
}
