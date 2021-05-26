# Minesweeper

![dotnet package](https://github.com/jonsofte/Minesweeper/workflows/dotnet%20package/badge.svg?branch=master)

Minesweeper game implemented in .net 5 and Vue

The game is hosted on <https://minesweepervue.azurewebsites.net/>

Container image on Dockerhub: <https://hub.docker.com/r/jonsofteland/minesweeper>

---

## Project structure and files

- **src/MinesweeperDomain**: Implementation of the core functionality of the game.
- **src/MinesweeperApi**: Rest API backend that handles game sessions.
- **src/MinesweeperConsole**: Application that is using the domain game functionality and handles the user interaction via console.
- **src/MinesweeperVue**: Vue/Vuetify frontend application that uses the backend API to play the game.
- **test/MinesweeperDomainTest** Unit tests that verifies the core implementation game logic in the domain project.
- **Dockerfile**: Builds a container image that contains both the backend api and the frontend application.
- **deployment/template.json** ARM template file for deployment to Azure.

---

## Minesweeper API & Minesweeper Vue

The Web Application consists of three components. The Domain project contains the core game logic. The Web API utilizes the domain functionality and exposes it with a game session handler through a REST API. The Minesweeper Vue frontend application calls the backend REST API and displays the game to the user.

The application is hosted as a docker container on Azure Web apps. When the master branch is updated on github, a github action workflow is triggered. The workflow builds and tests the code. If the build succeeds, it creates a single container image that contains both the backend API and the frontend Vue app. On completion of the build, the image is pushed to Dockerhub. A web hook on Dockerhub triggers a new deployment on Azure. The Azure Web App deployment will then download the new image and deploy it.

## Minesweeper Console

The Minesweeper console project is a simple implementation of a console front end. Example of the UI:

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
