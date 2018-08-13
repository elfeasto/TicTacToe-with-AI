using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTTT
{
    class FindResult
    {

        public static Result CheckResult(char[,] board, int targetInARow)
        {
            Result result;

            // check lines for a winner

            int ButtonsVertical = board.GetLength(1);
            int ButtonsAcross = board.GetLength(0);

            for (int i = 0; i < ButtonsVertical; i++)
            {
                for (int j = 0; j < ButtonsAcross; j++)
                {
                    int[] coords = new int[] { i, j };
                    result = CheckTile(board, coords, targetInARow);
                    if (result == Result.XWin)
                        return Result.XWin;
                    if (result == Result.OWin)
                        return Result.OWin;
                }
            }

            //check for draw 
            bool draw = true;
            foreach (char c in board)
            {
                if (c == ' ')
                {
                    draw = false;
                    break;
                }
            }
            if (draw)
            {
                return Result.Draw;
            }

            // neither win or draw so ongoing
            return Result.NotOver;

        }

        private static Result CheckTile(char[,] board, int[] Coords, int targetInaRow)
        {

            Result result;
            for (int XDirection = -1; XDirection <= 1; XDirection++)
            {
                for (int YDirection = -1; YDirection <= 1; YDirection++)
                {
                    if (XDirection == 0 && YDirection == 0)
                    {
                        continue;
                    }
                    else
                    {
                        int[] direction = new int[] { XDirection, YDirection };
                        result = CheckDirection(board, Coords, direction, targetInaRow);

                        if (result == Result.XWin)
                            return Result.XWin;
                        if (result == Result.OWin)
                            return Result.OWin;
                    }
                }
            } // end for loop

            return Result.NotOver;
        }

        private static Result CheckDirection(char[,] board, int[] coords, int[] direction, int targetInARow)
        {
            char tileChar = board[coords[0], coords[1]];
            if (tileChar == ' ')
            {
                return Result.NotOver;
            }
            else
            {
                int numInARow = 0;
                int[] currentCoords = new int[] { coords[0], coords[1] };


                bool coordsInGrid = true;
                while (coordsInGrid)
                {
                    if (board[currentCoords[0], currentCoords[1]] != tileChar)
                        break;
                    numInARow++;
                    currentCoords[0] += direction[0];
                    currentCoords[1] += direction[1];
                    coordsInGrid = IsInGrid(board, currentCoords);
                }



                if (numInARow >= targetInARow)
                {
                    if (tileChar == 'X')

                    {
                        return Result.XWin;
                    }
                    else
                    {
                        return Result.OWin;
                    }
                }
                else
                {
                    return Result.NotOver;
                }
            }
        }

        private static bool IsInGrid(char[,] board, int[] coords)
        {
            bool xInGrid = (coords[0] >= 0) && (coords[0] < board.GetLength(0));
            bool yInGrid = (coords[1] >= 0) && (coords[1] < board.GetLength(1));
            bool coordsInGrid = xInGrid && yInGrid;
            return coordsInGrid;
        }

    }
}
