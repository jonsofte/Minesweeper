using MediatR;
using Minesweeper.Tools;

namespace Minesweeper.Api.Queries
{
   public class GetGameQuery : IRequest<Result<MinesweeperApi.Models.Minesweeper>>
   {
      public string GameID { get; set; }
   }
}
