namespace MinesweeperApi
{
   public class GameSettingsOption
   {
      public const string GameSettings = "GameSettings";
      public Minefield MinefieldProperties { get; set; }
      public int MinutesBeforeDeletingOldGames { get; set; }

      public class Minefield
      {
         public int MaxWidth { get; set; }
         public int MaxHeight { get; set; }
      }
   }
}
