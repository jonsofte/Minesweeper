# Minesweeper

![dotnet package](https://github.com/jonsofte/Minesweeper/workflows/dotnet%20package/badge.svg?branch=master)

The Minesweeper game. Implemented with .net 5 and Vue

The game is hosted on Azure. It can be played at <https://minesweepervue.azurewebsites.net/>

## Project structure

- **src/MinesweeperDomain:** The core functionality of the game logic.
- **src/MinesweeperApi:** Rest API backend that expose game functionality and manage game sessions.
- **src/MinesweeperConsole:** Console application that handles a single game instance via command line.
- **src/MinesweeperVue:** Vue/Vuetify frontend application that uses the backend API to play the game.
- **test/MinesweeperDomainTest:** Unit tests that verifies that the game logic from the MinesweeperDomain project is correct.
- **deployment:** ARM template for deployment to Azure.

## Minesweeper Web Application

The Web Application is built from three components. The Domain project contains the core game functionality. The API backend component expose games with a session handler through a REST API. The Vue frontend interacts with the backend REST API and enables the user to play the game.

The application is hosted as a Docker container on Azure Web Apps. When a pull request is commited to the master branch on github, a github action will trigger the build and deployment workflow. If all tests are passing and all code compiles successfully, Docker will build a single container image that contains both the backend API and the frontend Vue app. On completion of the image build, the image is pushed to Dockerhub. A Webhook on Dockerhub will trigger a new deployment on Azure. The Azure Web App deployment will then download the new image and deploy it.

Container image on Dockerhub: <https://hub.docker.com/r/jonsofteland/minesweeper>

### Screenshots

![Minesweeper1](https://user-images.githubusercontent.com/24587666/119646565-99e33500-be1f-11eb-9819-bf294a675222.jpg)
![Minesweeper2](https://user-images.githubusercontent.com/24587666/119646569-9a7bcb80-be1f-11eb-859b-eb9e68d6cc99.jpg)

## Minesweeper Console

The Console project is a simple implementation of a command line frontend for the game:

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
