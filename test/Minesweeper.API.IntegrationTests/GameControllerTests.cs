using Newtonsoft.Json;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http.Json;
using System;
using MinesweeperApi.Models;

namespace Minesweeper.API.IntegrationTests
{
   public class GameControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
   {
      private readonly CustomWebApplicationFactory<Startup> _factory;

      public GameControllerTests(CustomWebApplicationFactory<Startup> factory)
      {
         _factory = factory;
      }

      [Fact]
      public async Task Return_list_of_ongoing_games()
      {
         var client = _factory.GetAnonymousClient();
         var response = await client.GetAsync("/game");

         Assert.True(response.IsSuccessStatusCode, $"Actual status code: {response.StatusCode}.");

         response.EnsureSuccessStatusCode();

         var responseString = await response.Content.ReadAsStringAsync();
         Assert.NotEmpty(responseString);
      }

      [Fact]
      public async Task Game_successfully_initializes()
      {
         var newGameRequest = new NewGameRequest()
         {
            NumberOfMines = 5,
            Width = 10,
            Height = 10
         };

         var client = _factory.GetAnonymousClient();
         var response = await client.PostAsJsonAsync("game/newgame/", newGameRequest);

         Assert.True(response.IsSuccessStatusCode, $"Actual status code: {response.StatusCode}.");

         response.EnsureSuccessStatusCode();
         var responseString = await response.Content.ReadAsStringAsync();
         var result = JsonConvert.DeserializeObject<MinesweeperApi.Models.Minesweeper>(responseString);

         Assert.IsType<MinesweeperApi.Models.Minesweeper>(result);
         Assert.True(Guid.TryParse(result.GameID, out Guid gameID));
         Assert.Equal("Active", result.GameStatus);
         Assert.Equal(newGameRequest.Width, result.Minefield.Width);
         Assert.Equal(newGameRequest.Height, result.Minefield.Height);
         Assert.Equal(newGameRequest.NumberOfMines, result.Minefield.NumberOfMines);
         Assert.Equal(0, result.NumberOfFieldsExplored);
         Assert.Equal(0, result.NumberOfFlagsUsed);
         Assert.Equal(14, result.Minefield.FieldTypeValues.Count);
      }

      [Fact]
      public async Task Retrieve_specific_game()
      {
         // Create new Game
         var newGameRequest = new NewGameRequest()
         {
            NumberOfMines = 5,
            Width = 10,
            Height = 10
         };

         var client = _factory.GetAnonymousClient();
         var newGameResponse = await client.PostAsJsonAsync("game/newgame/", newGameRequest);
         newGameResponse.EnsureSuccessStatusCode();
         var newGameResponseString = await newGameResponse.Content.ReadAsStringAsync();
         var newGameResult = JsonConvert.DeserializeObject<MinesweeperApi.Models.Minesweeper>(newGameResponseString);
         Guid createdGameID = Guid.Parse(newGameResult.GameID);

         // Retrieve newly created game
         var response = await client.GetAsync("/game/" + createdGameID);
         response.EnsureSuccessStatusCode();
         var responseString = await response.Content.ReadAsStringAsync();
         var result = JsonConvert.DeserializeObject<MinesweeperApi.Models.Minesweeper>(responseString);

         // Verify retrieved game
         Assert.IsType<MinesweeperApi.Models.Minesweeper>(result);
         Assert.True(Guid.TryParse(result.GameID, out Guid retievedGameID));
         Assert.Equal(createdGameID, retievedGameID);
         Assert.Equal("Active", result.GameStatus);
      }

      [Fact]
      public async Task Game_successfully_completes_when_all_none_mine_fields_are_explored()
      {
         // Create new Game
         var newGameRequest = new NewGameRequest()
         {
            NumberOfMines = 5,
            Width = 10,
            Height = 10
         };

         var client = _factory.GetAnonymousClient();
         var newGameResponse = await client.PostAsJsonAsync("game/newgame/", newGameRequest);
         newGameResponse.EnsureSuccessStatusCode();
         var newGameResponseString = await newGameResponse.Content.ReadAsStringAsync();
         var newGameResult = JsonConvert.DeserializeObject<MinesweeperApi.Models.Minesweeper>(newGameResponseString);
         Guid createdGameID = Guid.Parse(newGameResult.GameID);

         // Explore fields

         GameActionRequest actionRequest;
         actionRequest = new GameActionRequest() { X = 1, Y = 0, ActionType = MinesweeperApi.Models.Action.Explore };
         var response = await client.PostAsJsonAsync("/game/" + createdGameID, actionRequest);
         response.EnsureSuccessStatusCode();

         actionRequest = new GameActionRequest() { X = 1, Y = 1, ActionType = MinesweeperApi.Models.Action.Explore };
         response = await client.PostAsJsonAsync("/game/" + createdGameID, actionRequest);
         response.EnsureSuccessStatusCode();

         actionRequest = new GameActionRequest() { X = 1, Y = 5, ActionType = MinesweeperApi.Models.Action.Explore };
         response = await client.PostAsJsonAsync("/game/" + createdGameID, actionRequest);
         response.EnsureSuccessStatusCode();

         actionRequest = new GameActionRequest() { X = 5, Y = 5, ActionType = MinesweeperApi.Models.Action.Explore };
         response = await client.PostAsJsonAsync("/game/" + createdGameID, actionRequest);
         response.EnsureSuccessStatusCode();

         // Retrieve game
         response = await client.GetAsync("/game/" + createdGameID);
         response.EnsureSuccessStatusCode();
         var responseString = await response.Content.ReadAsStringAsync();
         var result = JsonConvert.DeserializeObject<MinesweeperApi.Models.Minesweeper>(responseString);

         // Validate that game is successfully completed
         Assert.IsType<MinesweeperApi.Models.Minesweeper>(result);
         Assert.Equal("EndedSuccess", result.GameStatus);
         Assert.Equal(95, result.NumberOfFieldsExplored);
         Assert.Equal(4, result.NumberOfMoves);
         Assert.Equal(0, result.NumberOfFlagsUsed);
      }

      [Fact]
      public async Task Game_successfully_completes_when_all_flags_are_set()
      {
         // Create new Game
         var newGameRequest = new NewGameRequest()
         {
            NumberOfMines = 5,
            Width = 10,
            Height = 10
         };

         var client = _factory.GetAnonymousClient();
         var newGameResponse = await client.PostAsJsonAsync("game/newgame/", newGameRequest);
         newGameResponse.EnsureSuccessStatusCode();
         var newGameResponseString = await newGameResponse.Content.ReadAsStringAsync();
         var newGameResult = JsonConvert.DeserializeObject<MinesweeperApi.Models.Minesweeper>(newGameResponseString);
         Guid createdGameID = Guid.Parse(newGameResult.GameID);

         // Flag fields
         GameActionRequest actionRequest;
         actionRequest = new GameActionRequest() { X = 0, Y = 0, ActionType = MinesweeperApi.Models.Action.Flag };
         var response = await client.PostAsJsonAsync("/game/" + createdGameID, actionRequest);
         response.EnsureSuccessStatusCode();

         actionRequest = new GameActionRequest() { X = 0, Y = 1, ActionType = MinesweeperApi.Models.Action.Flag };
         response = await client.PostAsJsonAsync("/game/" + createdGameID, actionRequest);
         response.EnsureSuccessStatusCode();

         actionRequest = new GameActionRequest() { X = 0, Y = 2, ActionType = MinesweeperApi.Models.Action.Flag };
         response = await client.PostAsJsonAsync("/game/" + createdGameID, actionRequest);
         response.EnsureSuccessStatusCode();

         actionRequest = new GameActionRequest() { X = 0, Y = 3, ActionType = MinesweeperApi.Models.Action.Flag };
         response = await client.PostAsJsonAsync("/game/" + createdGameID, actionRequest);
         response.EnsureSuccessStatusCode();

         actionRequest = new GameActionRequest() { X = 0, Y = 4, ActionType = MinesweeperApi.Models.Action.Flag };
         response = await client.PostAsJsonAsync("/game/" + createdGameID, actionRequest);
         response.EnsureSuccessStatusCode();

         // Retrieve game
         response = await client.GetAsync("/game/" + createdGameID);
         response.EnsureSuccessStatusCode();
         var responseString = await response.Content.ReadAsStringAsync();
         var result = JsonConvert.DeserializeObject<MinesweeperApi.Models.Minesweeper>(responseString);

         // Validate that game is successfully completed
         Assert.IsType<MinesweeperApi.Models.Minesweeper>(result);
         Assert.Equal("EndedSuccess", result.GameStatus);
         Assert.Equal(95, result.NumberOfFieldsExplored);
         Assert.Equal(0, result.NumberOfMoves);
         Assert.Equal(5, result.NumberOfFlagsUsed);
      }

      [Fact]
      public async Task Game_ends_when_mine_is_triggered()
      {
         // Create new Game
         var newGameRequest = new NewGameRequest()
         {
            NumberOfMines = 5,
            Width = 10,
            Height = 10
         };

         var client = _factory.GetAnonymousClient();
         var newGameResponse = await client.PostAsJsonAsync("game/newgame/", newGameRequest);
         newGameResponse.EnsureSuccessStatusCode();
         var newGameResponseString = await newGameResponse.Content.ReadAsStringAsync();
         var newGameResult = JsonConvert.DeserializeObject<MinesweeperApi.Models.Minesweeper>(newGameResponseString);
         Guid createdGameID = Guid.Parse(newGameResult.GameID);

         // Explore field with mine
         GameActionRequest actionRequest;
         actionRequest = new GameActionRequest() { X = 0, Y = 3, ActionType = MinesweeperApi.Models.Action.Explore };
         var response = await client.PostAsJsonAsync("/game/" + createdGameID, actionRequest);
         response.EnsureSuccessStatusCode();

         // Retrieve game
         response = await client.GetAsync("/game/" + createdGameID);
         response.EnsureSuccessStatusCode();
         var responseString = await response.Content.ReadAsStringAsync();
         var result = JsonConvert.DeserializeObject<MinesweeperApi.Models.Minesweeper>(responseString);

         // Validate that game is successfully ended
         Assert.IsType<MinesweeperApi.Models.Minesweeper>(result);
         Assert.Equal("EndedFailed", result.GameStatus);
      }
   }
}
