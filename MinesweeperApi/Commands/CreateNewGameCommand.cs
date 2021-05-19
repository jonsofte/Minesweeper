using MediatR;
using Minesweeper.Tools;
using System;

namespace Minesweeper.Api.Commands
{
   public class CreateNewGameCommand : IRequest<Result<string>>
   {
      public int Width { get; set; }
      public int Height { get; set; }
      public int NumberOfMines { get; set; }
   }
}
