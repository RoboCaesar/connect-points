using System;
using System.Collections.Generic;

namespace ConnectPoints
{
    class GameMap {
        public int columns, winLength;
        private bool finished = false;
        private int winningPlayer = 0;
        public List<int>[] gameColumns;

        public bool IsGameFinished()
        {
            return finished;
        }

        public void CongratulateWinner()
        {
            if (winningPlayer != 0)
            {
                Console.WriteLine("================================");
                Console.WriteLine("Congratulations Player " + winningPlayer + "!");
                Console.WriteLine("You win the game.");
                Console.WriteLine("================================");
            }
            else
            {
                Console.WriteLine("Wait a minute... I don't think anyone won the game yet.");
            }
        }

        public void InsertToken(int playerTurn, int col)
        {
            gameColumns[col - 1].Add(playerTurn); //index starts at zero, but the players don't see this.
            CheckForVictory(col);
        }

        public void PrintBoard()
        {
            Console.WriteLine("Game Board:");
            int maxListLength = 0;
            for (int i = 0; i < columns; i++)
            {
                if (i + 1 < 10) Console.Write((i + 1) + " |");
                else Console.Write((i + 1) + "|");
                if (gameColumns[i].Count > maxListLength) maxListLength = gameColumns[i].Count;
            }
            Console.WriteLine("");
            for (int i = 0; i < columns; i++)
            {
                Console.Write("---");
            }

            //after getting the max column height, start print out the columns
            if (maxListLength != 0)
            {
                for (int j = 0; j < maxListLength; j++)
                {
                    Console.WriteLine(""); //Get a new line
                    for (int i = 0; i < columns; i++)
                    {
                        if (gameColumns[i].Count > j) Console.Write(gameColumns[i][j] + "  ");
                        else Console.Write("-  ");
                    }
                }
            }
            Console.WriteLine("");
        }

        public int GetCoordVal(int row, int col)
        {
            if (col >= columns || col < 0 || row < 0) return -2; //-2 meaning out of bounds
            if (gameColumns[col].Count > row)
            {
                return gameColumns[col][row];
            }
            return -1; //-1 meaning nothing was found at that location.
        }

        public void CheckForVictory(int col)
        {
            for (int row = 0; row < gameColumns[col].Count; row++)
            {
                //First, check to the right and left.
                int player = gameColumns[col][row];
                int chainCount = 1;
                int offset = 1;
                bool hitWall = false;
                while (!hitWall)
                {
                    if (GetCoordVal(row, col + offset) == player) chainCount++;
                    else hitWall = true;
                    offset++;
                }
                offset = -1; hitWall = false;
                while (!hitWall)
                {
                    if (GetCoordVal(row, col + offset) == player) chainCount++;
                    else hitWall = true;
                    offset--;
                }
                //Console.WriteLine("Detected chain of " + player + ": " + chainCount);
                if (chainCount >= winLength)
                {
                    winningPlayer = player;
                    finished = true;
                    return;
                }
                //Then check up and down.
                chainCount = 1;
                offset = 1;
                hitWall = false;
                while (!hitWall)
                {
                    if (GetCoordVal(row + offset, col) == player) chainCount++;
                    else hitWall = true;
                    offset++;
                }
                offset = -1; hitWall = false;
                while (!hitWall)
                {
                    if (GetCoordVal(row + offset, col) == player) chainCount++;
                    else hitWall = true;
                    offset--;
                }
                //Console.WriteLine("Detected chain of " + player + ": " + chainCount);
                if (chainCount >= winLength)
                {
                    winningPlayer = player;
                    finished = true;
                    return;
                }

                //Then check diagonally (bottom-left to top-right)
                chainCount = 1;
                offset = 1;
                hitWall = false;
                while (!hitWall)
                {
                    if (GetCoordVal(row + offset, col -offset) == player) chainCount++;
                    else hitWall = true;
                    offset++;
                }
                offset = -1; hitWall = false;
                while (!hitWall)
                {
                    if (GetCoordVal(row + offset, col -offset) == player) chainCount++;
                    else hitWall = true;
                    offset--;
                }
                if (chainCount >= winLength)
                {
                    winningPlayer = player;
                    finished = true;
                    return;
                }

                //Finally check diagonally again (top-left to bottom-right)
                chainCount = 1;
                offset = 1;
                hitWall = false;
                while (!hitWall)
                {
                    if (GetCoordVal(row + offset, col + offset) == player) chainCount++;
                    else hitWall = true;
                    offset++;
                }
                offset = -1; hitWall = false;
                while (!hitWall)
                {
                    if (GetCoordVal(row + offset, col + offset) == player) chainCount++;
                    else hitWall = true;
                    offset--;
                }
                if (chainCount >= winLength)
                {
                    winningPlayer = player;
                    finished = true;
                    return;
                }
            }
        }

        public GameMap(int mapColumns, int chainLength) {
            columns = mapColumns;
            winLength = chainLength;
            gameColumns = new List<int>[columns];
            for (int i = 0; i < mapColumns; i++)
            {
                gameColumns[i] = new List<int>();
            }
        }
    }
    class MainClass
    {
        public static int GetNumber(int lowerBound, int upperBound, bool acceptNoInput = true)
        {
            bool enteredValidValue = false;
            int number = 0;

            while (!enteredValidValue)
            {
                Console.Write(">");
                string userInput = Console.ReadLine();
                bool checkUserInput = int.TryParse(userInput, out number);
                if (checkUserInput)
                {
                    if (number >= lowerBound && number <= upperBound)
                    {
                        enteredValidValue = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a value between " + lowerBound + " and " + upperBound);
                    }
                }
                else
                {
                    if (userInput == "" && acceptNoInput) return 0;
                    Console.WriteLine("Invalid input. Please enter a value between " + lowerBound + " and " + upperBound);
                }
            }
            return number;
        }

        public static void Main(string[] args)
        {
            bool finishedPlaying = false;

            while (!finishedPlaying)
            {
                Console.WriteLine("--===|  Welcome to Connect the Dots! |===--");
                Console.WriteLine("How many columns would you like? (default: 20)");
                int numOfColumns = GetNumber(2, 50);
                Console.WriteLine("How many tokens in a row will you need to win? (default: 4)");
                int chainLength = GetNumber(2, 50);

                if (numOfColumns == 0) numOfColumns = 20;
                if (chainLength == 0) chainLength = 4;
                GameMap game = new GameMap(numOfColumns, chainLength);
                int playerTurn = 1;

                while (!game.IsGameFinished())
                {
                    Console.WriteLine("");
                    Console.WriteLine("Player " + playerTurn + ", it's your turn!");
                    Console.WriteLine("Out of " + game.columns + " columns, which columns do you want drop a token into?");
                    int chooseCol = GetNumber(1, numOfColumns, false);
                    game.InsertToken(playerTurn, chooseCol);

                    game.PrintBoard();

                    game.CheckForVictory(chooseCol - 1);
                    //Change Turns
                    playerTurn = (playerTurn == 1 ? playerTurn = 2 : playerTurn = 1);
                }
                game.CongratulateWinner();

                Console.WriteLine("");
                Console.WriteLine("Would you like to play again? (Y/N)");
                var userChoice = Console.ReadKey();
                if (userChoice.KeyChar != 'Y' && userChoice.KeyChar != 'y') finishedPlaying = true;
            }


        }
    }
}
