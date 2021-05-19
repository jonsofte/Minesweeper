using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minesweeper.Tools;
using MinesweeperApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Minesweeper.Api.Commands
{
   public class CreateNewGameCommandHandler : IRequestHandler<CreateNewGameCommand, Result<string>>
   {
      private readonly ILogger<CreateNewGameCommandHandler> _logger;
      private readonly GameSettingsOption _gameSettings;
      private readonly GameFactory _gameFactory;
      private readonly IDictionary<string, GameSession> _games;

      public CreateNewGameCommandHandler(
         ILogger<CreateNewGameCommandHandler> logger,
         IDictionary<string, GameSession> games,
         IConfiguration configuration,
         GameFactory gamefactory
         )
      {
         _logger = logger;
         _gameSettings = configuration.GetSection(GameSettingsOption.GameSettings).Get<GameSettingsOption>();
         _gameFactory = gamefactory;
         _games = games;
      }

      public async Task<Result<string>> Handle(CreateNewGameCommand request, CancellationToken cancellationToken)
      {
         RemoveOldGamesIfExpired();

         var gameSession = new GameSession(game: _gameFactory.CreateNewGame());
         gameSession.StartGame(request.Width, request.Height, request.NumberOfMines);
          _games.Add(gameSession.Guid.ToString(), gameSession);

         _logger.LogInformation($"Starting new game: ({request.Width}x{request.Height} {request.NumberOfMines} mines)");
         return Result.Ok<string>(gameSession.Guid.ToString());
      }

      private void RemoveOldGamesIfExpired()
      {
         var expireDateTime = DateTime.Now.AddHours(-1 * _gameSettings.MinutesBeforeDeletingOldGames);
         var gamesToRemove = _games
            .Where(x => !x.Value.IsActive && x.Value.GameStartedTime < expireDateTime)
            .Select(s => s.Key)
            .ToList();

         if (gamesToRemove.Count > 0)
         {
            _logger.LogInformation($"Removing {gamesToRemove.Count} expired games");
            gamesToRemove.ForEach(x => _games.Remove(x));
         }
      }
   }
}
