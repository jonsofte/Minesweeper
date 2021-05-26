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
      private readonly IDictionary<string, GameSession> _games;

      public GetGameQueryHandler(
         IDictionary<string, GameSession> games
         )
      {
         _games = games;
      }
      public Task<Result<MinesweeperApi.Models.Minesweeper>> Handle(GetGameQuery request, CancellationToken cancellationToken)
      {
         return _games.ContainsKey(request.GameID) ?
            Task.FromResult(Result.Ok(_games[request.GameID].MapToModel())) :
            Task.FromResult(Result.Fail<MinesweeperApi.Models.Minesweeper>("Invalid Game ID"));
      }
   }
}
