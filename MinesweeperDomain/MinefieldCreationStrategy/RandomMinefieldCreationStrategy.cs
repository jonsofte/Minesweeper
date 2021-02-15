using System;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper.MinefieldCreationStrategy
{
   class RandomMinefieldCreationStrategy : IMinefieldCreationStrategy
   {
      public Land[,] CreateMinefield(int width, int height, int numberOfMines)
      {
         var random = new Random();
         Land[,] minefield = new Land[width, height];
         var mines = new HashSet<(int x, int y)>();

         while (mines.Count < numberOfMines) mines.Add((random.Next(width), random.Next(height)));
         mines.ToList().ForEach(mine => minefield[mine.x, mine.y] = Land.Mine);
         
         return minefield;
      }
   }
}
