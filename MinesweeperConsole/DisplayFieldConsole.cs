using Mineweeper;
using System;

namespace MinesweeperConsole
{
   internal class DisplayFieldConsole
   {
      private readonly Display[,] _display;

      public DisplayFieldConsole(Display[,] display)
      {
         _display = display;
      }

      public void printDisplay()
      {
         for (int y = 0; y < _display.GetLength(1); y++)
         {
            for (int x = 0; x < _display.GetLength(0); x++)
            {
               Console.Write(GetDisplay(_display[x, y]));

            }
            Console.Write('\n');
         }
      }

      private char GetDisplay(Display display) => display switch
      {
         Display.Empty => ' ',
         Display.One => '1',
         Display.Two => '2',
         Display.Three => '3',
         Display.Four => '4',
         Display.Five => '5',
         Display.Six => '6',
         Display.Seven => '7',
         Display.Eight => '8',
         Display.Explosion => '*',
         Display.Hidden => '#',
         _ => throw new ApplicationException("Invalid Display value")
      };
   }
}