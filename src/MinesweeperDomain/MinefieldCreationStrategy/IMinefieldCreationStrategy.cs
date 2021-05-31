namespace Minesweeper.MinefieldCreationStrategy
{
   // Strategy pattern for creation of mines in minefield
   public interface IMinefieldCreationStrategy
   {
      Land[,] CreateMinefield(int width, int height, int numberOfMines);
   }
}
