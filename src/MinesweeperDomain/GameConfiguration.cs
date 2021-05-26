using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
   public class GameConfiguration
   {
      public GameConfiguration(int width, int height, int numberOfMines)
      {
         Width = width;
         Height = height;
         NumberOfMines = numberOfMines;
      }

      public int Width { get; private set; }
      public int Height { get; private set; }
      public int NumberOfMines{ get; private set; }
   }
}
