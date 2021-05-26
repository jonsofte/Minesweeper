using Minesweeper.MinefieldCreationStrategy;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper
{
   public class MineField
   {
      public int Width { get; }
      public int Height { get; }
      public int NumberOfMines { get; }
      private readonly IMinefieldCreationStrategy _creationStrategy;
      private readonly Land[,] minefield;

      public MineField(int width, int height, int numberOfMines, IMinefieldCreationStrategy creationStrategy)
      {
         // TODO Validate input
         Width = width;
         Height = height;
         NumberOfMines = numberOfMines;
         _creationStrategy = creationStrategy;

         minefield = _creationStrategy.CreateMinefield(Width, Height, NumberOfMines);
      }

      public (bool exploded, int numberOfMines) ExploreLand(int x, int y)
      {
         if (minefield[x, y] == Land.Mine) return (true,0);
         return (false, GetNumberOfNeighbouringMines(x, y));
      }

      public bool IsMine(int x, int y) => minefield[x, y] == Land.Mine;

      public IReadOnlyList<(int x, int y)> GetMinePositions()
      {
         var mines = new List<(int x, int y)>();
         foreach (int x in Enumerable.Range(0,Width))
         {
            foreach (int y in Enumerable.Range(0, Height))
            {
               if (IsMine(x, y)) mines.Add((x, y));
            }
         }
         return mines;
      }

      private int GetNumberOfNeighbouringMines(int x, int y)
      {
         bool isValidPosition(int x, int y) => !(x < 0 || y < 0 || x >= Width || y >= Height);
         bool isMine(int x, int y) => isValidPosition(x, y) && minefield[x, y] == Land.Mine;

         int numberOfMines = 0;
         if (isMine(x-1, y-1)) numberOfMines++;
         if (isMine(x-1, y)) numberOfMines++;
         if (isMine(x-1, y+1)) numberOfMines++;
         if (isMine(x, y-1)) numberOfMines++;
         if (isMine(x, y+1)) numberOfMines++;
         if (isMine(x+1, y-1)) numberOfMines++;
         if (isMine(x+1, y)) numberOfMines++;
         if (isMine(x+1, y+1)) numberOfMines++;

         return numberOfMines;
      }
   }
}