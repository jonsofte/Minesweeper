using System;
using System.Collections.Generic;

namespace Minesweeper
{
   class DisplayField
   {
      private readonly MineField _minefield;
      private readonly int _width;
      private readonly int _height;
      private readonly Queue<(int x, int y)> fieldsToExplore;
      public Display[,] Display { get; }

      public DisplayField(MineField minefield)
      {
         _width = minefield.Width;
         _height = minefield.Height;
         _minefield = minefield;
         Display = new Display[_width, _height];
         fieldsToExplore = new Queue<(int x, int y)>();
      }

      public Display Explore(int x, int y)
      {
         AddToExploreQueueIfValidPoint(x, y);

         while (fieldsToExplore.Count > 0)
         {
            var position = fieldsToExplore.Dequeue();
            Display[position.x, position.y] = ExploreSingleField(position);
            if (Display[position.x, position.y] == Minesweeper.Display.Empty)
            {
               AddNeighbouringFields(position.x,position.y);
            }
         }

         return ExploreSingleField((x, y));

      }

      private void AddToExploreQueueIfValidPoint(int x, int y)
      {
         if (x < 0 || y < 0 || x >= _width || y >= _height) return;
         if (fieldsToExplore.Contains((x, y))) return;
         if (Display[x, y] == Minesweeper.Display.Hidden) fieldsToExplore.Enqueue((x, y));
      }

      private void AddNeighbouringFields(int x, int y)
      {
         AddToExploreQueueIfValidPoint(x - 1, y - 1);
         AddToExploreQueueIfValidPoint(x - 1, y);
         AddToExploreQueueIfValidPoint(x - 1, y + 1);
         AddToExploreQueueIfValidPoint(x, y - 1);
         AddToExploreQueueIfValidPoint(x, y + 1);
         AddToExploreQueueIfValidPoint(x + 1, y - 1);
         AddToExploreQueueIfValidPoint(x + 1, y);
         AddToExploreQueueIfValidPoint(x + 1, y + 1);
      }

      private Display ExploreSingleField((int x, int y) field)
      {
         if (Display[field.x, field.y] == Minesweeper.Display.Hidden)
         {
            var (exploded, numberOfMines) = _minefield.ExploreLand(field.x, field.y);
            if (exploded) return Minesweeper.Display.Explosion;
            return numberOfMines switch
            {
               0 => Minesweeper.Display.Empty,
               1 => Minesweeper.Display.One,
               2 => Minesweeper.Display.Two,
               3 => Minesweeper.Display.Three,
               4 => Minesweeper.Display.Four,
               5 => Minesweeper.Display.Five,
               6 => Minesweeper.Display.Six,
               7 => Minesweeper.Display.Seven,
               8 => Minesweeper.Display.Eight,
               _ => throw new ApplicationException("Invalid number of neighbouring mines")
            };
         }
         return Display[field.x, field.y];
      }
   }
}
