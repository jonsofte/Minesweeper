using Minesweeper.MinefieldCreationStrategy;
using System;

namespace Minesweeper
{
   public class GameFactory
   {
      readonly IMinefieldCreationStrategy _createionStrategy;

      public GameFactory(IMinefieldCreationStrategy createionStrategy)
      {
      _createionStrategy = createionStrategy;
      }

      public Game CreateNewGame() => new Game(_createionStrategy);
   }
}
