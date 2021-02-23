﻿using Minesweeper;
using Minesweeper.MinefieldCreationStrategy;
using System;

namespace MinesweeperConsole
{
   class Program
   {
      private static ConsoleGameStatus gameStatus;
      private static GameConfiguration config;
      private static Game minesweeper;
      private static(int x, int y) lastPointExplored;
      private static string ErrorMessage = "";

      static void Main()
      {
         gameStatus = ConsoleGameStatus.Initialization;
         minesweeper = new Game(new RandomMinefieldCreationStrategy());
         config = new GameConfiguration(width: 12, height: 10, numberOfMines: 8);
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
         var input = InputFunctions.GetCharacterInput("Start new game? (Y/N)");
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
               GetMinesweeperConfigurationFromInput();
               WriteCurrentConfiguration();
               gameStatus = ConsoleGameStatus.Active;
            }
            if (input.Value == 'n') gameStatus = ConsoleGameStatus.Active;
         }

         static void WriteCurrentConfiguration() => 
            Console.WriteLine($"Current configuration: Width: {config.Width} " +
            $"Height: {config.Height} Number Of mines: {config.NumberOfMines}");
      }

      private static void RunGame()
      {
         minesweeper.StartNewGame(config);
         DisplayFieldConsole display = new DisplayFieldConsole(minesweeper.Display);

         while (minesweeper.GameStatus == GameStatus.Active)
         {
            WriteScreen(display);
            ErrorMessage = "";

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
                     var exploreResult = minesweeper.Explore(result.Value.x, result.Value.y);
                     if (exploreResult.Failure) ErrorMessage = exploreResult.Error;
                  }
                  else ErrorMessage = result.Error;
                  break;
               case "f":
                  result = InputFunctions.GetPointFromInput();
                  if (result.Success)
                  {
                     var setFlagResult = minesweeper.SetFlag(result.Value.x, result.Value.y);
                     lastPointExplored = (result.Value.x + 1, result.Value.y + 1);
                     if (setFlagResult.Failure) ErrorMessage = setFlagResult.Error;
                  }
                  else ErrorMessage = result.Error;
                  break;
               case "u":
                  result = InputFunctions.GetPointFromInput();
                  if (result.Success)
                  {
                     var unsetFlagResult = minesweeper.UnSetFlag(result.Value.x, result.Value.y);
                     lastPointExplored = (result.Value.x + 1, result.Value.y + 1);
                     if (unsetFlagResult.Failure) ErrorMessage = unsetFlagResult.Error;
                  }
                  else ErrorMessage = result.Error;
                  break;
               case "q":
                  var abortResult = minesweeper.AbortGame();
                  if (abortResult.Failure) ErrorMessage = abortResult.Error;
                  break;
            }
         }

         WriteScreen(display);
         if (minesweeper.GameStatus == GameStatus.Aborted) 
            EndMessage("Game aborted");

         if (minesweeper.GameStatus == GameStatus.EndedFailed) 
            EndMessage($"Boom! Game ended. Mine exploded at {lastPointExplored.x},{lastPointExplored.y}");

         if (minesweeper.GameStatus == GameStatus.EndedSuccess) 
            EndMessage($"Congratulations! All mines found in {minesweeper.NumberOfMoves} moves. Game completed");

         static void EndMessage(String message)
         {
            Console.WriteLine(message + " (Press Enter to continue)");
            Console.ReadLine();
            gameStatus = ConsoleGameStatus.Initialization;
         }
      }

      static void GetMinesweeperConfigurationFromInput()
      {
         var width = InputFunctions.GetIntegerInput("Enter grid width:");
         var height = InputFunctions.GetIntegerInput("Enter grid height:");
         var numberOfMines = InputFunctions.GetIntegerInput("Enter number of mines:");
       
         if (height.Success && width.Success && numberOfMines.Success)
            config = new GameConfiguration(width.Value, height.Value, numberOfMines.Value);
      }

      private static void ExploreRandom()
      {
         Random rand = new Random();
         var (x, y) = (rand.Next(config.Width - 1), rand.Next(config.Height - 1));
         var exploreResult = minesweeper.Explore(x, y);
         if (exploreResult.Failure) ErrorMessage = exploreResult.Error;
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
         Console.WriteLine($"Minefield: width: {config.Width} height: {config.Height} mines: {config.NumberOfMines}");
         Console.WriteLine($"Number of moves: {minesweeper.NumberOfMoves} " +
            $"Fields explored: {minesweeper.NumberOfFieldsExplored}/{minesweeper.NumberOfFields} Flags used: {minesweeper.NumberOfFlagsUsed}" );
         Console.WriteLine($"Last point explored: { lastPointExplored.x},{ lastPointExplored.y}");
         Console.WriteLine($"Status: {GetGameStatusString(minesweeper.GameStatus)}");
         if (!String.IsNullOrEmpty(ErrorMessage))
            Console.WriteLine(ErrorMessage);
      }

      private static string GetGameStatusString(GameStatus status) => status switch
      {
         GameStatus.Active => "Active",
         GameStatus.EndedFailed => "Game Over - Mine Exploded!",
         GameStatus.EndedSuccess => "Game Completed! Congratulations!",
         GameStatus.Aborted => "Game aborted",
         GameStatus.Uninitialized => "Uninitialized",
         _ => throw new ApplicationException("Invalid Game status")
      };
   }
}