using System;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper
{
   class DisplayField
   {
      private readonly MineField _minefield;
      private readonly int _width;
      private readonly int _height;
      private readonly Queue<(int x, int y)> fieldsToExplore;
      public Display[,] DisplayGrid { get; }

      public DisplayField(MineField minefield)
      {
         _width = minefield.Width;
         _height = minefield.Height;
         _minefield = minefield;
         DisplayGrid = new Display[_width, _height];
         fieldsToExplore = new Queue<(int x, int y)>();
      }

      public Display Explore(int x, int y)
      {
         AddToExploreQueueIfValidPoint(x, y);

         while (fieldsToExplore.Count > 0)
         {
            var position = fieldsToExplore.Dequeue();
            DisplayGrid[position.x, position.y] = ExploreSingleField(position);
            if (DisplayGrid[position.x, position.y] == Display.Empty)
            {
               AddNeighbouringFields(position.x,position.y);
            }
         }
         return ExploreSingleField((x, y));
      }

      public void SetFlag(int x, int y)
      {
         if (DisplayGrid[x, y] == Display.Hidden) DisplayGrid[x, y] = Display.Flagged;
      }

      public void UnSetFlag(int x, int y)
      {
         if (DisplayGrid[x, y] == Display.Flagged) DisplayGrid[x, y] = Display.Hidden;
      }

      public bool AllMinesFoundOrFlagged()
      {
         var summary = CreateFieldSummary();
         int count = 0;
         if (summary.ContainsKey(Display.Hidden)) count += summary[Display.Hidden];
         if (summary.ContainsKey(Display.Flagged)) count += summary[Display.Flagged];
         return (count == _minefield.NumberOfMines);
      }

      public bool AllMinesFlagged()
      {
         int numberOfMatches = 0;

         for (int x = 0; x < DisplayGrid.GetLength(0); x++)
            for (int y = 0; y < DisplayGrid.GetLength(1); y++)
               if (DisplayGrid[x, y] == Display.Flagged && _minefield.IsMine(x, y)) numberOfMatches++;

         return numberOfMatches == _minefield.NumberOfMines;
      }

      public int NumberOfFlagsUsed()
      {
         var fieldSummary = CreateFieldSummary();
         return fieldSummary.ContainsKey(Display.Flagged) ? fieldSummary[Display.Flagged] : 0;
      }

      public int NumberOfFieldsExplored()
      {
         var summary = CreateFieldSummary();
         int numberOfFields = _width * _height;
         if (summary.ContainsKey(Display.Hidden)) numberOfFields -= summary[Display.Hidden];
         if (summary.ContainsKey(Display.Flagged)) numberOfFields -= summary[Display.Flagged];
         return numberOfFields;
      }

      private void AddToExploreQueueIfValidPoint(int x, int y)
      {
         if (x < 0 || y < 0 || x >= _width || y >= _height) return;
         if (fieldsToExplore.Contains((x, y))) return;
         if (DisplayGrid[x, y] == Display.Hidden) fieldsToExplore.Enqueue((x, y));
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
         if (DisplayGrid[field.x, field.y] == Display.Hidden)
         {
            var (exploded, numberOfMines) = _minefield.ExploreLand(field.x, field.y);
            if (exploded) return Display.Explosion;
            return numberOfMines switch
            {
               0 => Display.Empty,
               1 => Display.One,
               2 => Display.Two,
               3 => Display.Three,
               4 => Display.Four,
               5 => Display.Five,
               6 => Display.Six,
               7 => Display.Seven,
               8 => Display.Eight,
               _ => throw new ApplicationException("Invalid number of neighbouring mines")
            };
         }
         return DisplayGrid[field.x, field.y];
      }

      private Dictionary<Display, int> CreateFieldSummary()
      {
         var displayValues = new List<Display>();
         for (int x = 0; x < DisplayGrid.GetLength(0); x++)
            for (int y = 0; y < DisplayGrid.GetLength(1); y++)
               displayValues.Add(DisplayGrid[x, y]);

         var summary = displayValues.GroupBy(x => x)
            .Select(group => new { Type = group.Key, Count = group.Count()});

         return summary.ToDictionary(x => x.Type, x => x.Count);
      }
   }
}
