using Microsoft.Extensions.DependencyInjection;
using Minesweeper.MinefieldCreationStrategy;

namespace Minesweeper.API.IntegrationTests
{
   static class TestsServiceRegistration
   {
      // Service registration for Gamefactory. 
      // Will create new games with a predictable minefield layout.
      public static IServiceCollection AddMinefieldTestGame(this IServiceCollection services)
      {
         services.AddSingleton<IMinefieldCreationStrategy, EveryFifthFieldMinefieldCreationStrategy>();
         services.AddSingleton<GameFactory>();
         return services;
      }
   }
}