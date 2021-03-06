﻿using Microsoft.Extensions.DependencyInjection;
using Minesweeper;
using Minesweeper.Domain;
using Minesweeper.MinefieldCreationStrategy;
using System;

namespace MinesweeperConsole
{
   class Program
   {
      private static ConsoleGameStatus gameStatus;
      private static GameConfiguration _config;
      private static Game _minesweeper;
      private static GameFactory _gameFactory;
      private static(int x, int y) lastPointExplored;
      private static string ErrorMessage = "";

      static void Main()
      {
         gameStatus = ConsoleGameStatus.Initialization;
         _gameFactory = InitiateGameFactory();
         _minesweeper = _gameFactory.CreateNewGame();
         _config = new GameConfiguration(width: 12, height: 10, numberOfMines: 8);
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
            Console.WriteLine($"Current configuration: Width: {_config.Width} " +
            $"Height: {_config.Height} Number Of mines: {_config.NumberOfMines}");
      }

      private static void RunGame()
      {
         _minesweeper.StartNewGame(_config);
         DisplayFieldConsole display = new DisplayFieldConsole(_minesweeper.Display);

         while (_minesweeper.GameStatus == GameStatus.Active)
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
                     var exploreResult = _minesweeper.Explore(result.Value.x, result.Value.y);
                     if (exploreResult.Failure) ErrorMessage = exploreResult.Error;
                  }
                  else ErrorMessage = result.Error;
                  break;
               case "f":
                  result = InputFunctions.GetPointFromInput();
                  if (result.Success)
                  {
                     var setFlagResult = _minesweeper.SetFlag(result.Value.x, result.Value.y);
                     lastPointExplored = (result.Value.x + 1, result.Value.y + 1);
                     if (setFlagResult.Failure) ErrorMessage = setFlagResult.Error;
                  }
                  else ErrorMessage = result.Error;
                  break;
               case "u":
                  result = InputFunctions.GetPointFromInput();
                  if (result.Success)
                  {
                     var unsetFlagResult = _minesweeper.UnSetFlag(result.Value.x, result.Value.y);
                     lastPointExplored = (result.Value.x + 1, result.Value.y + 1);
                     if (unsetFlagResult.Failure) ErrorMessage = unsetFlagResult.Error;
                  }
                  else ErrorMessage = result.Error;
                  break;
               case "q":
                  var abortResult = _minesweeper.AbortGame();
                  if (abortResult.Failure) ErrorMessage = abortResult.Error;
                  break;
            }
         }

         WriteScreen(display);
         if (_minesweeper.GameStatus == GameStatus.Aborted) 
            EndMessage("Game aborted");

         if (_minesweeper.GameStatus == GameStatus.EndedFailed) 
            EndMessage($"Boom! Game ended. Mine exploded at {lastPointExplored.x},{lastPointExplored.y}");

         if (_minesweeper.GameStatus == GameStatus.EndedSuccess) 
            EndMessage($"Congratulations! All mines found in {_minesweeper.NumberOfMoves} moves. Game completed");

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
            _config = new GameConfiguration(width.Value, height.Value, numberOfMines.Value);
      }

      private static void ExploreRandom()
      {
         Random rand = new Random();
         var (x, y) = (rand.Next(_config.Width - 1), rand.Next(_config.Height - 1));
         var exploreResult = _minesweeper.Explore(x, y);
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
         Console.WriteLine($"Minefield: width: {_config.Width} height: {_config.Height} mines: {_config.NumberOfMines}");
         Console.WriteLine($"Number of moves: {_minesweeper.NumberOfMoves} " +
            $"Fields explored: {_minesweeper.NumberOfFieldsExplored}/{_minesweeper.NumberOfFields} Flags used: {_minesweeper.NumberOfFlagsUsed}" );
         Console.WriteLine($"Last point explored: { lastPointExplored.x},{ lastPointExplored.y}");
         Console.WriteLine($"Status: {GetGameStatusString(_minesweeper.GameStatus)}");
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

      private static GameFactory InitiateGameFactory()
      {
         ServiceCollection services = new ServiceCollection();
         services.AddMinefieldGame();
         return (GameFactory)ActivatorUtilities.CreateInstance(services.BuildServiceProvider(), typeof(GameFactory));
      }
   }
}