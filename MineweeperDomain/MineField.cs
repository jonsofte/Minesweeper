namespace Mineweeper
{
   public class MineField
   {
      private readonly int _width;
      private readonly int _height;
      private readonly int _numberOfMines;
      private readonly IMinefieldCreationStrategy _creationStrategy;
      private readonly Land[,] minefield;

      public MineField(int width, int height, int numberOfMines, IMinefieldCreationStrategy creationStrategy)
      {
         // TODO Validate input
         _width = width;
         _height = height;
         _numberOfMines = numberOfMines;
         _creationStrategy = creationStrategy;

         minefield = _creationStrategy.CreateMinefield(_width, _height, _numberOfMines);
      }

      public (bool exploded, int numberOfMines) ExploreLand(int x, int y)
      {
         if (minefield[x, y] == Land.Mine) return (true,0);
         return (false, GetNumberOfNeighbouringMines(x, y));
      }

      private int GetNumberOfNeighbouringMines(int x, int y)
      {
         bool isValidPosition(int x, int y) => !(x < 0 || y < 0 || x >= _width || y >= _height);
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