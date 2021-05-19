using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Minesweeper.Tools;
using MinesweeperApi;

namespace Minesweeper.Api.Queries
{
   public class GetGameQueryHandler : IRequestHandler<GetGameQuery, Result<MinesweeperApi.Models.Minesweeper>>
   {
      private readonly ILogger<GetGameQueryHandler> _logger;
      private readonly IDictionary<string, GameSession> _games;

      public GetGameQueryHandler(
         ILogger<GetGameQueryHandler> logger,
         IDictionary<string, GameSession> games
         )
      {
         _logger = logger;
         _games = games;
      }
      public async Task<Result<MinesweeperApi.Models.Minesweeper>> Handle(GetGameQuery request, CancellationToken cancellationToken)
      {
         return _games.ContainsKey(request.GameID) ?
            Result.Ok(_games[request.GameID].MapToModel()) :
            Result.Fail<MinesweeperApi.Models.Minesweeper>("Invalid Game ID");
      }
   }
}
