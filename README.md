# Minesweeper

![dotnet package](https://github.com/jonsofte/Minesweeper/workflows/dotnet%20package/badge.svg?branch=master)

Simple implementation of the minesweeper game in .net 5

## Console

Currently only a console interface for the game is implemented:

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
