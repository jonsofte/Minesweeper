using MediatR;
using Minesweeper.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minesweeper.Api.Commands
{
   public class ExecuteActionCommand : IRequest<Result>
   {
      public string GameId { get; set; }
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
