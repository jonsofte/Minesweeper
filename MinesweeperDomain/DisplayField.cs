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

      public List<int> GetDisplayFieldsAsList()
      {
         var displayList = new List<int>();

         for (int y = 0; y < DisplayGrid.GetLength(1); y++)
            for (int x = 0; x < DisplayGrid.GetLength(0); x++)
               displayList.Add((int)DisplayGrid[x, y]);

         return displayList;
      }

      public bool AllMinesFoundOrFlagged()
      {
         var summary = NumberOfDisplayTypesInGrid();
         int count = 0;
         if (summary.ContainsKey(Display.Hidden)) count += summary[Display.Hidden];
         if (summary.ContainsKey(Display.Flagged)) count += summary[Display.Flagged];
         return (count == _minefield.NumberOfMines);
      }

      public bool AllMinesFlagged()
      {
         if (NumberOfFlagsUsed() > _minefield.NumberOfMines) return false;
         int numberOfMatches = 0;

         for (int x = 0; x < DisplayGrid.GetLength(0); x++)
            for (int y = 0; y < DisplayGrid.GetLength(1); y++)
               if (DisplayGrid[x, y] == Display.Flagged && _minefield.IsMine(x, y)) numberOfMatches++;

         return numberOfMatches == _minefield.NumberOfMines;
      }

      public void RevealAllMines()
      {
         var mines = _minefield.GetMinePositions();

         foreach (var mine in mines)
         {
            if (DisplayGrid[mine.x, mine.y] != Display.Explosion)
               DisplayGrid[mine.x, mine.y] = Display.DiscoveredMine;
         }
      }

      public void RevealNonFlaggedMinesAndEmptyFields()
      {
         var mines = _minefield.GetMinePositions();

         foreach (int x in Enumerable.Range(0, _width))
         {
            foreach (int y in Enumerable.Range(0, _height))
            {
               if (DisplayGrid[x,y] == Display.Hidden)
               {
                  if (mines.Contains((x, y))) DisplayGrid[x, y] = Display.DiscoveredMine;
                  else DisplayGrid[x, y] = ExploreSingleField((x, y));
               }
            }
         }
      }

      public int NumberOfFlagsUsed()
      {
         var displaySummary = NumberOfDisplayTypesInGrid();
         return displaySummary.ContainsKey(Display.Flagged) ? displaySummary[Display.Flagged] : 0;
      }

      public int NumberOfFieldsExplored()
      {
         var summary = NumberOfDisplayTypesInGrid();
         int numberOfFields = _width * _height;
         if (summary.ContainsKey(Display.Hidden)) numberOfFields -= summary[Display.Hidden];
         if (summary.ContainsKey(Display.Flagged)) numberOfFields -= summary[Display.Flagged];
         if (summary.ContainsKey(Display.DiscoveredMine)) numberOfFields -= summary[Display.DiscoveredMine];
         return numberOfFields;
      }

      public int NumberOfFields() => _width * _height;

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

      private Dictionary<Display, int> NumberOfDisplayTypesInGrid()
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
