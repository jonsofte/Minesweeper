using Microsoft.Extensions.DependencyInjection;
using Minesweeper.MinefieldCreationStrategy;

namespace Minesweeper.UnitTests
{
   static class TestsServiceRegistration
   {
      public static IServiceCollection AddMinefieldGame(this IServiceCollection services)
      {
         services.AddSingleton<IMinefieldCreationStrategy, EveryFifthFieldMinefieldCreationStrategy>();
         services.AddSingleton<GameFactory>();
         return services;
      }
   }
}
