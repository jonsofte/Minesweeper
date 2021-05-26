using MediatR;
using Microsoft.Extensions.Logging;
using Minesweeper.Tools;
using MinesweeperApi;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Minesweeper.Api.Commands
{
   public class ExecuteActionCommandHandler : IRequestHandler<ExecuteActionCommand, Result>
   {
      private readonly ILogger<ExecuteActionCommandHandler> _logger;
      private readonly IDictionary<string, GameSession> _games;

      public ExecuteActionCommandHandler(
         ILogger<ExecuteActionCommandHandler> logger,
         IDictionary<string, GameSession> games
         )
      {
         _logger = logger;
         _games = games;
      }

      public Task<Result> Handle(ExecuteActionCommand request, CancellationToken cancellationToken)
      {
         _logger.LogInformation($"Game: {request.GameId} {request.ActionType} {request.X}x{request.Y}");
         if (!_games.ContainsKey(request.GameId)) return Task.FromResult(Result.Fail("Invalid Game ID"));
         var gameSession = _games[request.GameId];
         switch (request.ActionType)
         {
            case Action.Explore:
               var result = gameSession.Explore(request.X, request.Y);
               if (result.Failure) return Task.FromResult(Result.Fail(result.Error));
               break;
            case Action.Flag:
               result = gameSession.SetFlag(request.X, request.Y);
               if (result.Failure) return Task.FromResult(Result.Fail(result.Error));
               break;
            case Action.Unflag:
               result = gameSession.UnSetFlag(request.X, request.Y);
               if (result.Failure) return Task.FromResult(Result.Fail(result.Error));
               break;
            case Action.Quit:
               result = gameSession.AbortGame();
               if (result.Failure) return Task.FromResult(Result.Fail(result.Error));
               break;
            default:
               return Task.FromResult(Result.Fail("Invalid action type"));
         }
         return Task.FromResult(Result.Ok());
      }

   }
}
