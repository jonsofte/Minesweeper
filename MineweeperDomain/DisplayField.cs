using System;
using System.Collections.Generic;

namespace Mineweeper
{
   class DisplayField
   {
      private readonly MineField _minefield;
      private readonly int _width;
      private readonly int _height;
      public Display[,] Display { get; }

      public DisplayField(MineField minefield, int width, int height)
      {
         _width = width;
         _height = height;
         _minefield = minefield;
         Display = new Display[width, height];
      }

      public Display Explore(int x, int y)
      {
         var fieldsToExplore = new Queue<(int x, int y)>();
         AddField(x, y);

         //if (currentField == Mineweeper.Display.Explosion) return currentField;
         //if (currentField != Mineweeper.Display.Empty) return currentField;         

         while (fieldsToExplore.Count > 0)
         {
            var position = fieldsToExplore.Dequeue();
            Display current = ExploreSingleField(position);
            Display[position.x, position.y] = current;
            if (current == Mineweeper.Display.Empty)
            {
               addExploringFields(position.x,position.y);
            }
         }

         return ExploreSingleField((x, y));

         void AddField(int x, int y)
         {
            if (x < 0 || y < 0 || x >= _width || y >= _height) return;
            if (fieldsToExplore.Contains((x, y))) return;
            if (Display[x, y] == Mineweeper.Display.Hidden) fieldsToExplore.Enqueue((x, y));
         }

         void addExploringFields(int x, int y) 
         {
            AddField(x - 1, y - 1);
            AddField(x - 1, y);
            AddField(x - 1, y + 1);
            AddField(x, y - 1);
            AddField(x, y + 1);
            AddField(x + 1, y - 1);
            AddField(x + 1, y);
            AddField(x + 1, y+1);
         }
      }

      public Display ExploreSingleField((int x, int y) m)
      {
         if (Display[m.x, m.y] == Mineweeper.Display.Hidden)
         {
            var (exploded, numberOfMines) = _minefield.ExploreLand(m.x, m.y);
            if (exploded) return Mineweeper.Display.Explosion;
            return numberOfMines switch
            {
               0 => Mineweeper.Display.Empty,
               1 => Mineweeper.Display.One,
               2 => Mineweeper.Display.Two,
               3 => Mineweeper.Display.Three,
               4 => Mineweeper.Display.Four,
               5 => Mineweeper.Display.Five,
               6 => Mineweeper.Display.Six,
               7 => Mineweeper.Display.Seven,
               8 => Mineweeper.Display.Eight,
               _ => throw new ApplicationException("Invalid number of neighbouring mines")
            };
         }
         return Display[m.x, m.y];
      }
   }
}
