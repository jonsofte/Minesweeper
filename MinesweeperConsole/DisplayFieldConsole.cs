using Minesweeper;
using System;
using System.Linq;

namespace MinesweeperConsole
{
   internal class DisplayFieldConsole
   {
      private readonly Display[,] _display;

      public DisplayFieldConsole(Display[,] display)
      {
         _display = display;
      }

      public void PrintDisplay()
      {
         int yWidth = _display.GetLength(1);
         int xWitdh = _display.GetLength(0);

         Enumerable.Range(0, xWitdh.ToString().Length).ToList().ForEach(x => WriteLineForNumberPosition(xWitdh, x));

         for (int y = 0; y < yWidth; y++)
         {
            Console.Write("{0,3} ", y+1);
            for (int x = 0; x < xWitdh; x++)
            {
               Console.Write(GetDisplay(_display[x, y]));
            }
            Console.Write('\n');
         }
      }

      private static void WriteLineForNumberPosition(int width, int position)
      {
         Console.Write("    ");
         Enumerable.Range(1, width).ToList().ForEach(x => Console.Write(GetValueInNumberPosition(x, position,width.ToString().Length)));
         Console.WriteLine();
      }

      private static char GetValueInNumberPosition(int number, int position, int maxLength)
      {
         string value = number.ToString().PadLeft(maxLength);
         if (position >= value.Length) return ' ';
         return value[position];
      }

      private static char GetDisplay(Display display) => display switch
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
         Display.Flagged => 'F',
         Display.DiscoveredMine => 'M',
         _ => throw new ApplicationException("Invalid display value")
      };
   }
}