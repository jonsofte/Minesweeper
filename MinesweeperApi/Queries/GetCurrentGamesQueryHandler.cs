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
      private readonly ILogger<GetCurrentGamesQuery> _logger;
      private readonly IDictionary<string, GameSession> _games;
      public GetCurrentGamesQueryHandler(
         ILogger<GetCurrentGamesQuery> logger,
         IDictionary<string, GameSession> games
         )
      {
         _logger = logger;
         _games = games;
      }

      public async Task<IReadOnlyList<MinesweeperApi.Models.Minesweeper>> Handle(GetCurrentGamesQuery request, CancellationToken cancellationToken)
      {
         return _games.Select(x => x.Value.MapToModel()).ToList();
      }
   }
}
