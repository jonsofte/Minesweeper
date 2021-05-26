using Microsoft.Extensions.DependencyInjection;
using Minesweeper.MinefieldCreationStrategy;

namespace Minesweeper.Domain
{
   public static class MinesweeperServiceRegistration
   {
      public static IServiceCollection AddMinefieldGame(this IServiceCollection services)
      {
         services.AddSingleton<IMinefieldCreationStrategy, RandomMinefieldCreationStrategy>();
         services.AddSingleton<GameFactory>();     
         return services;
      }
   }
}
