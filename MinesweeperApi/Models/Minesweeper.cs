using System;
using System.Collections.Generic;

namespace MinesweeperApi.Models
{
   public class Minesweeper
   {
      public string GameID { get; set; }
      public string GameStatus { get; set; }
      public DateTimeOffset GameStartedTime { get; set; }
      public List<GameEvent> GameMoves { get; set; }
      public int NumberOfMoves { get; set; }
      public int NumberOfFieldsExplored { get; set; }
      public int NumberOfFlagsUsed { get; set; }
      public Minefield Minefield { get; set; }
   }

   public class Minefield
   {
      public int NumberOfFields { get; set; }
      public int Width { get; set; }
      public int Height { get; set; }
      public int NumberOfMines { get; set; }
      public List<int> GridData { get; set; }
      public Dictionary<int,string> FieldTypeValues { get; set; }
   }

   public class GameEvent
   {
      public DateTimeOffset Timestamp { get; set; }
      public string Description { get; set; }
   }
}
