using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MyTTT
{   
 
    class AI
    {
        public int numCalculations = 0;

        public AI()
        {

        }

        // unit test for 3 by 3 boards
        public void testOne()
        {
            char[,] testBoard = new char[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    testBoard[i, j] = ' ';
                }
            }

            testBoard[0, 0] = 'X';
            testBoard[1, 0] = 'X';
            testBoard[2, 0] = ' ';
            testBoard[0, 1] = 'O';
            testBoard[1, 1] = 'O';
            testBoard[2, 1] = ' ';
            testBoard[0, 2] = 'X';
            testBoard[1, 2] = 'X';
            testBoard[2, 2] = ' ';

            

            int[] nextMove = GetAIMove(testBoard, true);

            string message = "Next Move is " + nextMove[0].ToString() + ", " + nextMove[1].ToString();

            MessageBox.Show(message);
        }

        public int[] GetAIMove(char[,] board, bool xTurn)
        {
            /// searches throught empty tiles to either
            /// minimise or maximise the score
            /// which gives the best move.
            int bestMoveScore;
            int currentMoveScore;
            int[] bestMove = { -1, -1 };

            numCalculations = 0;

            if (xTurn) //maximise score
            {
                bestMoveScore = -100;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == ' ')
                        {
                            int[] coords = { i, j };
                            char[,] updatedBoard = GetUpdatedBoard(board, coords, xTurn);
                            currentMoveScore = MinMax(updatedBoard, !xTurn, true);

                            //string message = "Coords: " + coords[0].ToString() + "," + coords[1].ToString() + "\n Score: " + currentMoveScore.ToString();
                            //MessageBox.Show(message);

                            if (currentMoveScore > bestMoveScore)
                            {
                                bestMoveScore = currentMoveScore;
                                bestMove = coords;
                            }
                        }
                    }
                }
            }
            else //minimise score
            {
                bestMoveScore = 100;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == ' ')
                        {
                            int[] coords = { i, j };
                            char[,] updatedBoard = GetUpdatedBoard(board, coords, xTurn);

                            currentMoveScore = MinMax(updatedBoard, !xTurn, false);

                            //string message = "Coords: " + coords[0].ToString() + "," + coords[1].ToString() + "\n Score: " + currentMoveScore.ToString();

                            //MessageBox.Show(message);

                            if (currentMoveScore < bestMoveScore)
                            {
                                bestMoveScore = currentMoveScore;
                                bestMove = coords;
                            }
                        }
                    }
                }
            }

            //string message2 = "bestMoveScore: " + bestMoveScore.ToString();
            //MessageBox.Show(message2);
        
            return bestMove;
        }

        private int MinMax(char[,] board, bool XTurn, bool isMaximisingPlayer)
        /// Uses a minmax algorithm modified to include depth
        /// The depth means the AI will prolong a losing game
        /// and end a winning game as quickly as possible
        {
            //check if over
            Result currentResult = FindResult.CheckResult(board, 3);
            if (currentResult != Result.NotOver) //means game is over
            {
                numCalculations++;
                int depth = GetBoardDepth(board);
                return GetScore(currentResult, isMaximisingPlayer, depth);
            }

            if (XTurn) //maximising turn
            {
                int maxValue = -100;
                int value;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == ' ')
                        {
                            char[,] updatedBoard = GetUpdatedBoard(board, new int[] { i, j }, XTurn);
                            value = MinMax(updatedBoard, !XTurn, isMaximisingPlayer);
                            if (value > maxValue)
                                maxValue = value;
                        }
                    }
                }
                return maxValue;
            }
            else
            {
                int minValue = 100;
                int value;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == ' ')
                        {
                            char[,] updatedBoard = GetUpdatedBoard(board, new int[] { i, j }, XTurn);
                            value = MinMax(updatedBoard, !XTurn, isMaximisingPlayer);
                            if (value < minValue)
                                minValue = value;
                        }
                    }
                }
                return minValue;
            }
        }

        private int GetScore(Result result, bool isMaximisingPlayer, int currentDepth)
        {
            int score;
            if (result == Result.XWin)
            {
                if (isMaximisingPlayer)
                {
                    return 10 - currentDepth;
                }
                else
                {
                    return 10 + currentDepth;
                }
            }

            else if (result == Result.Draw)
                score = 0;

            else if (result == Result.OWin)
            {
                if (isMaximisingPlayer)
                {
                    return -10 + currentDepth;
                }
                else
                {
                    return -10 - currentDepth;
                }
            }
               
            else
            {
                score = 1234;
                MessageBox.Show("Error in GetScore method");
            }

            return score;
        }

        public int[] GetAIMove(char[,] board, bool xTurn, int maxDepth)
        {
            /// searches throught empty tiles to either
            /// minimise or maximise the score
            /// which gives the best move.
            int bestMoveScore;
            int currentMoveScore;
            int[] bestMove = { -1, -1 };

            numCalculations = 0;

            if (xTurn) //maximise score
            {
                bestMoveScore = -100;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == ' ')
                        {
                            int[] coords = { i, j };
                            char[,] updatedBoard = GetUpdatedBoard(board, coords, xTurn);
                            currentMoveScore = MinMax(updatedBoard, !xTurn, true, maxDepth);

                            //string message = "Coords: " + coords[0].ToString() + "," + coords[1].ToString() + "\n Score: " + currentMoveScore.ToString();
                            //MessageBox.Show(message);

                            if (currentMoveScore > bestMoveScore)
                            {
                                bestMoveScore = currentMoveScore;
                                bestMove = coords;
                            }
                        }
                    }
                }
            }
            else //minimise score
            {
                bestMoveScore = 100;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == ' ')
                        {
                            int[] coords = { i, j };
                            char[,] updatedBoard = GetUpdatedBoard(board, coords, xTurn);

                            currentMoveScore = MinMax(updatedBoard, !xTurn, false, maxDepth);

                            //string message = "Coords: " + coords[0].ToString() + "," + coords[1].ToString() + "\n Score: " + currentMoveScore.ToString();

                            //MessageBox.Show(message);

                            if (currentMoveScore < bestMoveScore)
                            {
                                bestMoveScore = currentMoveScore;
                                bestMove = coords;
                            }
                        }
                    }
                }
            }

            //string message2 = "bestMoveScore: " + bestMoveScore.ToString();
            //MessageBox.Show(message2);

            return bestMove;
        }

        private int MinMax(char[,] board, bool XTurn, bool isMaximisingPlayer, int maxDepth)
        {
            //check if over
            Result currentResult = FindResult.CheckResult(board, 3);
            if (currentResult != Result.NotOver) //means game is over
            {
                numCalculations++;
                int depth = GetBoardDepth(board);

                return GetScore(currentResult, isMaximisingPlayer, depth);
            }

            if (XTurn) //maximising turn
            {
                int maxValue = -100;
                int value;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == ' ')
                        {
                            char[,] updatedBoard = GetUpdatedBoard(board, new int[] { i, j }, XTurn);
                            value = MinMax(updatedBoard, !XTurn, isMaximisingPlayer);
                            if (value > maxValue)
                                maxValue = value;
                        }
                    }
                }
                return maxValue;
            }
            else
            {
                int minValue = 100;
                int value;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == ' ')
                        {
                            char[,] updatedBoard = GetUpdatedBoard(board, new int[] { i, j }, XTurn);
                            value = MinMax(updatedBoard, !XTurn, isMaximisingPlayer);
                            if (value < minValue)
                                minValue = value;
                        }
                    }
                }
                return minValue;
            }
        }

        private int GetScore(Result result, bool isMaximisingPlayer, int currentDepth, int maxDepth)
        {
            int score = 0;
            

            return score;
        }


        private static char[,] GetUpdatedBoard(char[,] board, int[] coords, bool xTurn)
        {
            int numAcross = board.GetLength(0);
            int numDown = board.GetLength(1);
            char[,] updatedBoard = new char[numAcross, numDown];

            for (int i = 0; i < numAcross; i++)
            {
                for (int j = 0; j < numDown; j++)
                {
                    updatedBoard[i, j] = board[i, j];
                }
            }


            if (xTurn)
                updatedBoard[coords[0], coords[1]] = 'X';
            else
                updatedBoard[coords[0], coords[1]] = 'O';

            return updatedBoard;

        }

        private static void showBoard(char[,] board)
        {

            string message = "";
            for (int i = 2; i >= 0  ; i--)
            {
                for (int j = 0; j < 3; j++)
                {
                    message += " " + board[j, i];

                }
                message += "\n";
            }

            MessageBox.Show(message);
        }

        private int GetBoardDepth(char[,] board)
        {
            int depth = 0;

            foreach (char c in board)
            {
                if (c != ' ')
                    depth++;
            }
            return depth;

        }
    }
}
