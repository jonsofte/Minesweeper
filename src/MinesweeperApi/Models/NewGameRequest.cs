namespace MinesweeperApi.Models
{
   public class NewGameRequest
   {
      public int Width { get; set; }
      public int Height { get; set; }
      public int NumberOfMines { get; set; }
   }
}
