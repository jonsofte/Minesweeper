using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;
using Minesweeper.Tools;
using System;

namespace MinesweeperConsole
{
   class Program
   {
      static GameConfiguration config;
      static Game minesweeper;
      static (int x, int y) lastPoint;

      static void Main()
      {
         //minesweeper = new Game(new RandomMinefieldCreationStrategy());
         minesweeper = new Game(new EveryFifthFieldMinefieldCreationStrategy());
         config = new GameConfiguration() { Width = 10, Height = 10, NumberOfMines = 5 };
         lastPoint = (0, 0);

         minesweeper.StartNewGame(config.Width, config.Height, config.NumberOfMines);
         DisplayFieldConsole display = new DisplayFieldConsole(minesweeper.Display);

         PrintConfiguration();
         Console.WriteLine("Start new Game? (Y/N)");

         while (minesweeper.GameStatus == GameStatus.Active)
         {
            Console.Clear();
            PrintStatus();
            display.PrintDisplay();
            Console.WriteLine("Command: (R)andom (E)xplore (F)lagg (U)nflagg (Q)uit");
            string input = Console.ReadLine().ToLower().Trim();

            switch (input) 
            {
               case "r":
                  ExploreRandom();
                  break;
               case "e":
                  var result = GetPointFromInput();
                  if (result.Success)
                  {
                     lastPoint = (result.Value.x + 1, result.Value.y + 1);
                     minesweeper.Explore(result.Value.x,result.Value.y);
                  }
                  else Console.WriteLine("Invalid input");
                  break;
               case "f":
                  result = GetPointFromInput();
                  if (result.Success)
                  {
                     minesweeper.SetFlag(result.Value.x, result.Value.y);
                     lastPoint = (result.Value.x + 1, result.Value.y + 1);
                  }
                  else Console.WriteLine("Invalid input");
                  break;
               case "u":
                  result = GetPointFromInput();
                  if (result.Success)
                  {
                     minesweeper.UnSetFlag(result.Value.x, result.Value.y);
                     lastPoint = (result.Value.x + 1, result.Value.y + 1);
                  }
                  else Console.WriteLine("Invalid input");
                  break;
               case "q":
                  minesweeper.AbortGame();
                  break;
            }
         }
         if (minesweeper.GameStatus == GameStatus.EndedSuccess)
         {
            Console.WriteLine("Congratulations! Game complete");
            display.PrintDisplay();
         }
      }


      private static void ExploreRandom()
      {
         Random rand = new Random();
         var (x, y) = (rand.Next(config.Width - 1), rand.Next(config.Height - 1));
         minesweeper.Explore(x, y);
         lastPoint = (x + 1, y + 1);
      }

      private static Result<(int x ,int y)> GetPointFromInput()
      {
         try 
         { 
            Console.WriteLine("Enter coordinates: x,y");
            string input = Console.ReadLine();
            var values = input.Split(',',StringSplitOptions.TrimEntries);
            return Result.Ok((Int32.Parse(values[0]) - 1, Int32.Parse(values[1]) - 1));
         }
         catch (Exception)
         {
            return Result.Fail<(int, int)>("Invalid Input");
         }
         
      }

      private static void PrintStatus()
      {
         Console.WriteLine($"Number Of Moves: {minesweeper.NumberOfMoves} Fields Explored: {minesweeper.NumberOfFieldsExplored()} Status: {minesweeper.GameStatus} Last Point Explored: {lastPoint.x},{lastPoint.y}");
         Console.WriteLine("-------------------------------------------------------------");
      }

      private static void PrintConfiguration()
      {
         Console.WriteLine($"Current Configuration: Width: {config.Width} Height: {config.Height} Number Of Mines: {config.NumberOfMines}");
      }
   }
}
