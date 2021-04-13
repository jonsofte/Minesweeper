using Minesweeper.MinefieldCreationStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
   public class GameFactory
   {
      readonly IMinefieldCreationStrategy _strategy;

      public GameFactory()
      {
         _strategy = new RandomMinefieldCreationStrategy();
      }

      public Game CreateNewGame() => new Game(_strategy);
   }
}
