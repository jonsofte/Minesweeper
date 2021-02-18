# Minesweeper

![dotnet package](https://github.com/jonsofte/Minesweeper/workflows/dotnet%20package/badge.svg?branch=master)

Simple implementation of the minesweeper game in .net 5

## Console

Currently only a console interface for the game is implemented:

      ----------------------------------------------------------
      Minefield properties: width: 40 height: 15 mines: 30
      Number Of Moves: 6 Fields Explored: 556 Flags used: 3
      Status: Active - Last Point Explored: 12,1
      ----------------------------------------------------------
                  1111111111222222222233333333334
          1234567890123456789012345678901234567890
        1        1###F1                    111
        2     111111111                    1#1
        3     1#1                          111
        4     111            122211
        5          111     113####1  111 111 111
        6          1#2211  1###3211  1#1 1#1 1#1
        7 111      1####1  11211 111 111 111 111
        8 1F1 111  1#221211      1#21  111
        9 111 1#1  1#1  2#2     12##1  1#1
      10     111  111  2#2     1###1  2#2
      11               111     112#1  1F1
      12                   111   1#1  111
      13                   1#1   111          111
      14             111111111                1#1
      15             1####1                   111
      ----------------------------------------------------------
      Command: (R)andom (E)xplore (F)lag (U)nflag (Q)uit