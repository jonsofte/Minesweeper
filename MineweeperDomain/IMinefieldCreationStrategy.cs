namespace Mineweeper
{
   public interface IMinefieldCreationStrategy
   {
      Land[,] CreateMinefield(int width, int height, int numberOfMines);
   }
}
