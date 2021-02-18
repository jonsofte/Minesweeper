using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;
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
         config = new GameConfiguration() { Width = 12, Height = 10, NumberOfMines = 8 };
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
         var input = InputFunctions.GetCharacterInput("Start new Game? (Y/N)");
         if (input.Success)
         {
            if (input.Value == 'y') gameStatus = ConsoleGameStatus.Configuration;
            if (input.Value == 'n') gameStatus = ConsoleGameStatus.Ended;
         }
      }

      private static void Configuration()
      {          
         WriteCurrentConfiguration();
         var input = InputFunctions.GetCharacterInput("Change game configuration? (Y/N)");
         if (input.Success)
         {
            if (input.Value == 'y')
            {
               var gridWidthResult = InputFunctions.GetIntegerInput("Enter grid width:");
               var gridHeightResult = InputFunctions.GetIntegerInput("Enter grid height:");
               var numberOfMinesResult = InputFunctions.GetIntegerInput("Enter number of mines:");
               if (gridHeightResult.Success && gridWidthResult.Success && numberOfMinesResult.Success)
               {
                  config = new GameConfiguration() { 
                     Width = gridWidthResult.Value, 
                     Height = gridHeightResult.Value, 
                     NumberOfMines = numberOfMinesResult.Value 
                  };
               }
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
            WriteScreen(display);

            Console.WriteLine("Command: (R)andom (E)xplore (F)lag (U)nflag (Q)uit");
            string stringInput = Console.ReadLine().ToLower().Trim();

            switch (stringInput)
            {
               case "r":
                  ExploreRandom();
                  break;
               case "e":
                  var result = InputFunctions.GetPointFromInput();
                  if (result.Success)
                  {
                     lastPointExplored = (result.Value.x + 1, result.Value.y + 1);
                     minesweeper.Explore(result.Value.x, result.Value.y);
                  }
                  else Console.WriteLine("Invalid input");
                  break;
               case "f":
                  result = InputFunctions.GetPointFromInput();
                  if (result.Success)
                  {
                     minesweeper.SetFlag(result.Value.x, result.Value.y);
                     lastPointExplored = (result.Value.x + 1, result.Value.y + 1);
                  }
                  else Console.WriteLine("Invalid input");
                  break;
               case "u":
                  result = InputFunctions.GetPointFromInput();
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

         WriteScreen(display);
         if (minesweeper.GameStatus == GameStatus.Aborted) 
            EndMessage("Game Aborted");

         if (minesweeper.GameStatus == GameStatus.EndedFailed) 
            EndMessage($"Boom! Game Ended. Mine exploded at {lastPointExplored.x},{lastPointExplored.y}");

         if (minesweeper.GameStatus == GameStatus.EndedSuccess) 
            EndMessage($"Congratulations! All mines found in {minesweeper.NumberOfMoves} moves. Game completed");

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

      static void WriteScreen(DisplayFieldConsole display)
      {
         Console.Clear();
         Console.WriteLine("----------------------------------------------------------------");
         PrintStatus();
         Console.WriteLine("----------------------------------------------------------------");
         display.PrintDisplay();
         Console.WriteLine("----------------------------------------------------------------");
      }

      private static void PrintStatus()
      {
         Console.WriteLine($"Minefield properties: width: {config.Width} height: {config.Height} mines: {config.NumberOfMines}");
         Console.WriteLine($"Number Of Moves: {minesweeper.NumberOfMoves} Fields Explored: {minesweeper.NumberOfFieldsExplored} Flags used: {minesweeper.NumberOfFlagsUsed}" );
         Console.WriteLine($"Status: {minesweeper.GameStatus} - Last Point Explored: {lastPointExplored.x},{lastPointExplored.y}");
      }
   }
}