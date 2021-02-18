using Minesweeper.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperConsole
{
   class InputFunctions
   {
      internal static Result<(int x, int y)> GetPointFromInput()
      {
         try
         {
            Console.WriteLine("Enter coordinates: x,y");
            string input = Console.ReadLine().Trim();
            var values = input.Split(',', StringSplitOptions.TrimEntries);
            return Result.Ok((Int32.Parse(values[0]) - 1, Int32.Parse(values[1]) - 1));
         }
         catch (Exception)
         {
            return Result.Fail<(int, int)>("Invalid Input");
         }
      }

      internal static Result<char> GetCharacterInput(string requestMessage)
      {
         try
         {
            Console.WriteLine(requestMessage);
            char input = char.Parse(Console.ReadLine().Trim().ToLower());
            return Result.Ok(input);
         }
         catch (Exception)
         {
            return Result.Fail<char>("Invalid Input");
         }
      }

      internal static Result<int> GetIntegerInput(string requestMessage)
      {
         try
         {
            Console.WriteLine(requestMessage);
            int input = int.Parse(Console.ReadLine().Trim().ToLower());
            return Result.Ok(input);
         }
         catch (Exception)
         {
            return Result.Fail<int>("Invalid Input");
         }
      }
   }
}
