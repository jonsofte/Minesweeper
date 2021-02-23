
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MinesweeperApi.Models
{
   public class GameActionRequest
   {
      [Required]
      public Action ActionType { get; set; }
      public int X { get; set; }
      public int Y { get; set; }
   }

   public enum Action
   {
      Explore,
      Flag,
      Unflag,
      Quit
   }
}
