using MediatR;
using Microsoft.Extensions.Logging;
using MinesweeperApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Minesweeper.Api.Queries
{
   public class GetCurrentGamesQueryHandler : IRequestHandler<GetCurrentGamesQuery, IReadOnlyList<MinesweeperApi.Models.Minesweeper>>
   {
      private readonly IDictionary<string, GameSession> _games;
      public GetCurrentGamesQueryHandler(
         IDictionary<string, GameSession> games
         )
      {
         _games = games;
      }

      public Task<IReadOnlyList<MinesweeperApi.Models.Minesweeper>> Handle(GetCurrentGamesQuery request, CancellationToken cancellationToken)
      {
         return Task.FromResult((IReadOnlyList<MinesweeperApi.Models.Minesweeper>)_games.Select(x => x.Value.MapToModel()).ToList());
      }
   }
}
