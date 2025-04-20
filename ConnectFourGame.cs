using System;

namespace ConnectFour
{
    class Program
    {
        static void Main(string[] args)
        {
            GameController controller = new GameController();
            controller.StartGame();
        }
    }

    public class GameController
    {
        private Player player1;
        private Player player2;
        private Board gameBoard;

        public GameController()
        {
            gameBoard = new Board();
        }

        public void StartGame()
        {
            Console.Write("Enter Player 1 name: ");
            string name1 = Console.ReadLine() ?? "Player 1";

            Console.Write("Enter Player 2 name: ");
            string name2 = Console.ReadLine() ?? "Player 2";

            player1 = new HumanPlayer(name1, 'X');
            player2 = new HumanPlayer(name2, 'O');

            Player currentPlayer = player1;

            while (true)
            {
                gameBoard.Display();
                Console.WriteLine($"{currentPlayer.Name}'s Turn ({currentPlayer.Symbol})");

                int column = currentPlayer.GetMove();
                if (column == -1) 
                {
                    Console.WriteLine("Invalid input. Try again.");
                    continue;
                }

                bool success = gameBoard.DropDisc(column, currentPlayer.Symbol);

                if (!success)
                {
                    Console.WriteLine("Column full. Try again.");
                    continue;
                }

                if (gameBoard.CheckWin(currentPlayer.Symbol))
                {
                    gameBoard.Display();
                    Console.WriteLine($"{currentPlayer.Name} wins!");
                    break;
                }

                if (gameBoard.IsFull())
                {
                    gameBoard.Display();
                    Console.WriteLine("It's a draw!");
                    break;
                }

                currentPlayer = (currentPlayer == player1) ? player2 : player1;
            }
        }
    }

    public class Board
    {
        private const int Rows = 6;
        private const int Columns = 7; 
        private char[,] grid = new char[Rows, Columns];

        public Board()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    grid[r, c] = '.';
        }

        public bool DropDisc(int column, char symbol)
        {
            if (column < 0 || column >= Columns) return false; 

            for (int r = Rows - 1; r >= 0; r--)
            {
                if (grid[r, column] == '.')
                {
                    grid[r, column] = symbol;
                    return true;
                }
            }

            return false; 
        }

        public bool IsFull()
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[0, c] == '.')
                    return false;
            }

            return true;
        }

        public bool CheckWin(char symbol)
        {
            // Horizontal
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns - 3; c++)
                    if (grid[r, c] == symbol && grid[r, c + 1] == symbol && grid[r, c + 2] == symbol && grid[r, c + 3] == symbol)
                        return true;

            // Vertical
            for (int r = 0; r < Rows - 3; r++)
                for (int c = 0; c < Columns; c++)
                    if (grid[r, c] == symbol && grid[r + 1, c] == symbol && grid[r + 2, c] == symbol && grid[r + 3, c] == symbol)
                        return true;

            // Diagonal (bottom left to top right)
            for (int r = 3; r < Rows; r++)
                for (int c = 0; c < Columns - 3; c++)
                    if (grid[r, c] == symbol && grid[r - 1, c + 1] == symbol && grid[r - 2, c + 2] == symbol && grid[r - 3, c + 3] == symbol)
                        return true;

            // Diagonal (top left to bottom right)
            for (int r = 0; r < Rows - 3; r++)
                for (int c = 0; c < Columns - 3; c++)
                    if (grid[r, c] == symbol && grid[r + 1, c + 1] == symbol && grid[r + 2, c + 2] == symbol && grid[r + 3, c + 3] == symbol)
                        return true;

            return false;
        }

        public void Display()
        {
            Console.Clear();
            Console.WriteLine(" 0 1 2 3 4 5 6"); // Adjusted column numbers
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    Console.Write($" {grid[r, c]}");
                }
                Console.WriteLine();
            }
        }
    }

    public abstract class Player
    {
        public string Name { get; }
        public char Symbol { get; }

        public Player(string name, char symbol)
        {
            Name = name;
            Symbol = symbol;
        }

        public abstract int GetMove();
    }

    public class HumanPlayer : Player
    {
        public HumanPlayer(string name, char symbol) : base(name, symbol) { }

        public override int GetMove()
        {
            int col;
            while (true)
            {
                Console.Write("Enter column (0-6): "); // Adjusted prompt
                string input = Console.ReadLine();
                if (int.TryParse(input, out col) && col >= 0 && col <= 6) // Adjusted validation
                    return col;
                Console.WriteLine("Invalid input. Try again.");
            }
        }
    }
}