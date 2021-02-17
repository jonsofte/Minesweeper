using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;
using Minesweeper.Tools;
using System;

namespace MinesweeperConsole
{
   class Program
   {
      static ConsoleGameStatus gameStatus;
      static GameConfiguration config;
      static Game minesweeper;
      static (int x, int y) lastPointExplored;

      static void Main()
      {
         gameStatus = ConsoleGameStatus.Initialization;
         minesweeper = new Game(new RandomMinefieldCreationStrategy());
         config = new GameConfiguration() { Width = 10, Height = 10, NumberOfMines = 5 };
         lastPointExplored = (0, 0);

         while (gameStatus != ConsoleGameStatus.Ended) { 
            switch (gameStatus)
            {
               case ConsoleGameStatus.Initialization:
                  Initialization();
                  break;
               case ConsoleGameStatus.Configuration:
                  Configuration();
                  break;
               case ConsoleGameStatus.Active:
                  RunGame();
                  break;
            }
         }
      }

      private static void Initialization()
      {
         var input = GetCharacterInput("Start new Game? (Y/N)");
         if (input.Success)
         {
            if (input.Value == 'y') gameStatus = ConsoleGameStatus.Configuration;
            if (input.Value == 'n') gameStatus = ConsoleGameStatus.Ended;
         }
      }

      private static void Configuration()
      {          
         WriteCurrentConfiguration();
         var input = GetCharacterInput("Change game configuration? (Y/N)");
         if (input.Success)
         {
            if (input.Value == 'y')
            {
               // Todo - update configuration values
               WriteCurrentConfiguration();
               gameStatus = ConsoleGameStatus.Active;
            }
            if (input.Value == 'n') gameStatus = ConsoleGameStatus.Active;
         }

         static void WriteCurrentConfiguration() => Console.WriteLine($"Current Configuration: Width: {config.Width} Height: {config.Height} Number Of Mines: {config.NumberOfMines}");
      }

      private static void RunGame()
      {
         minesweeper.StartNewGame(config.Width, config.Height, config.NumberOfMines);
         DisplayFieldConsole display = new DisplayFieldConsole(minesweeper.Display);

         while (minesweeper.GameStatus == GameStatus.Active)
         {
            Console.Clear();
            PrintStatus();
            display.PrintDisplay();
            Console.WriteLine("Command: (R)andom (E)xplore (F)lagg (U)nflagg (Q)uit");
            string stringInput = Console.ReadLine().ToLower().Trim();

            switch (stringInput)
            {
               case "r":
                  ExploreRandom();
                  break;
               case "e":
                  var result = GetPointFromInput();
                  if (result.Success)
                  {
                     lastPointExplored = (result.Value.x + 1, result.Value.y + 1);
                     minesweeper.Explore(result.Value.x, result.Value.y);
                  }
                  else Console.WriteLine("Invalid input");
                  break;
               case "f":
                  result = GetPointFromInput();
                  if (result.Success)
                  {
                     minesweeper.SetFlag(result.Value.x, result.Value.y);
                     lastPointExplored = (result.Value.x + 1, result.Value.y + 1);
                  }
                  else Console.WriteLine("Invalid input");
                  break;
               case "u":
                  result = GetPointFromInput();
                  if (result.Success)
                  {
                     minesweeper.UnSetFlag(result.Value.x, result.Value.y);
                     lastPointExplored = (result.Value.x + 1, result.Value.y + 1);
                  }
                  else Console.WriteLine("Invalid input");
                  break;
               case "q":
                  minesweeper.AbortGame();
                  break;
            }
         }

         if (minesweeper.GameStatus == GameStatus.EndedSuccess) EndMessage("Congratulations! Game complete");
         if (minesweeper.GameStatus == GameStatus.EndedFailed) EndMessage("Boom! Game Ended");
         if (minesweeper.GameStatus == GameStatus.Aborted) EndMessage("Game Aborted");

         static void EndMessage(String message)
         {
            Console.WriteLine(message + " (Press Enter to continue)");
            Console.ReadLine();
            gameStatus = ConsoleGameStatus.Initialization;
         }
      }

      private static void ExploreRandom()
      {
         Random rand = new Random();
         var (x, y) = (rand.Next(config.Width - 1), rand.Next(config.Height - 1));
         minesweeper.Explore(x, y);
         lastPointExplored = (x + 1, y + 1);
      }

      private static Result<(int x ,int y)> GetPointFromInput()
      {
         try 
         { 
            Console.WriteLine("Enter coordinates: x,y");
            string input = Console.ReadLine().Trim();
            var values = input.Split(',',StringSplitOptions.TrimEntries);
            return Result.Ok((Int32.Parse(values[0]) - 1, Int32.Parse(values[1]) - 1));
         }
         catch (Exception)
         {
            return Result.Fail<(int, int)>("Invalid Input");
         }
      }

      private static Result<char> GetCharacterInput(string requestMessage)
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

      private static void PrintStatus()
      {
         Console.WriteLine($"Number Of Moves: {minesweeper.NumberOfMoves} Fields Explored: {minesweeper.NumberOfFieldsExplored()} Status: {minesweeper.GameStatus} Last Point Explored: {lastPointExplored.x},{lastPointExplored.y}");
         Console.WriteLine("-------------------------------------------------------------");
      }
   }
}
