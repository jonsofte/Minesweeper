# Minesweeper

![dotnet package](https://github.com/jonsofte/Minesweeper/workflows/dotnet%20package/badge.svg?branch=master)

A simple implementation of the minesweeper game in .net 5 and vue

**Hosted on <https://minesweepervue.azurewebsites.net/>**

(Might take some seconds to start, due to the use of the Free App Service Plan)

## Minesweeper Domain and Tests

The **MinesweeperDomain** is the core minesweeper game implementation. The **MinesweeperDomainTest** project contains unit tests that verifies the core implementation.

## Minesweeper API & Minesweeper Vue

The **MinesweeperAPI** is an ASP.Net WebAPI implementation that depends on the Minesweeper Core library to expose the game through a REST API. **MinesweeperVue** is the web front end that utilizes the backend REST API to display the game. The front end is implemented with Vue, Vuetify and Vuex.

## CI/CD

New commits triggers the Github action in the **.github** folder that builds and tests the solution and builds the **Dockerfile** container image and posts it to Docker Hub. A webhook on Docker Hub initializes a new deloyment on Azure Web Apps, that updates the application container image.

Container image on Docker hub: <https://hub.docker.com/r/jonsofteland/minesweeper>

## Minesweeper Console

The **MinesweeperConsole** project is an implementation of a  Console front end that is using the Minesweeper Domain core functionaltiy. Example of the UI:

      ----------------------------------------------------------------
      Minefield: width: 40 height: 15 mines: 40
      Number of moves: 9 Fields explored: 496/600 Flags used: 2
      Last point explored: 11,1
      Status: Active
      ----------------------------------------------------------------
                   1111111111222222222233333333334
          1234567890123456789012345678901234567890
        1 #####1   111      1#1
        2 #####1   1F1      1#1           111
        3 ####21   111      11211     111 1#1
        4 ####1              1##1     1#1 111
        5 ####1        111   1#21   11211
        6 ####1        1F1   1#1 1222#1
        7 ####1  111   1221111#111##211
        8 11211  1#1    1####1######1   111111
        9   1#1  111 122212#2111####1   1####1
      10   111 111  1##1 111  1####1   1111221
      11       1#1  13#31     1####2       1#1
      12       111   1##1     1####1      1221
      13             1221   122####1      1#1  11
      14    111   111       1######1      111  1#
      15    1#1   1#1       12#####1           1#
      ----------------------------------------------------------------
      Command: (R)andom (E)xplore (F)lag (U)nflag (Q)uit
