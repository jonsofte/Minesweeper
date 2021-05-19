using MediatR;
using System.Collections.Generic;

namespace Minesweeper.Api.Queries
{
   public class GetCurrentGamesQuery : IRequest<IReadOnlyList<MinesweeperApi.Models.Minesweeper>>
   {
   }
}
