
namespace OnlineTicTacToe
{
    static class GameServer
    {
        // main vars
        public static string[] board { get; set; } = { " ", " " , " ", " ", " ", " ", " ", " ", " " };
        public static bool gameTie { get; private set; }

        // render the board at the beginng
        public static void RenderBeginningBoard()
        {
            Console.WriteLine("| 0 | 1 | 2|");
            Console.WriteLine("| 3 | 4 | 5|");
            Console.WriteLine("| 6 | 7 | 8|");
        }

        // render the board with the current moves
        public static void RenderBoard()
        {
            Console.WriteLine($"| {board[0]} | {board[1]} | {board[2]} | \n");
            Console.WriteLine($"| {board[3]} | {board[4]} | {board[5]} | \n");
            Console.WriteLine($"| {board[6]} | {board[7]} | {board[8]} | \n");
        }

        public static void Engine(this Stream stream)
        {
            RenderBeginningBoard();
            while (true)
            {
                ReWrite: // the ReWrite: is so that if the move have already been put the player will need to try again
                Console.Write($"$~ Enter a position from 0-8: ");
                int turn = Convert.ToInt32(Console.ReadLine());
                // check if the space is open
                if (CheckIfOpen(turn))
                {
                    Protocol.WriteString(stream, Convert.ToString(turn));
                    board[turn] = "X";
                    RenderBoard();
                    checkTie(); // check the game for a possibile tie

                    // check the lines for x
                    if (CheckXLines())
                    {
                        Console.WriteLine("You won");
                        Thread.Sleep(5000);
                        ReWriteBoard();
                        break;
                    }
                    else if (gameTie) // check if the gameTie boolean is true
                    {
                        Console.WriteLine("The game is a draw");
                        Thread.Sleep(5000);
                        ReWriteBoard();
                        break;
                    }
                }
                else
                {
                    goto ReWrite; // ask again
                }

                /*
                from now on the client will start
                how it works: 
                    - read the turn from the client and render board
                    - as we did with the server, but now we will check the lines for 0 
                    - check if the game is tie
                    - if nothing from here is true, it will just keep going with the while loop, otherwise the loop from this piece of code will break and the other will start
                */
                 
                Console.WriteLine("Client's turn...");

                var turnServer = Protocol.ReadString(stream);
                board[Convert.ToInt32(turnServer)] = "0";
                RenderBoard();
                checkTie();

                if (Check0Lines())
                {
                    Console.WriteLine("The client won");
                    Thread.Sleep(5000);
                    ReWriteBoard();
                    break;
                }
                else if (gameTie)
                {
                    Console.WriteLine("The game is a draw");
                    Thread.Sleep(5000);
                    ReWriteBoard();
                    break;
                } else
                {
                    continue;
                }


            }
        }
        
        // restart the game
        public static void RestartGame(this Stream stream)
        {
            Console.Clear();
            Engine(stream);
        }

        public static void checkTie()
        {
            /*
            it will set the gameTie boolean to true:
                - if every item on the board has been written and 0 or X have not won
                - otherwise, the gameTie boolean will be false
            */
              
            

            foreach (var item in board)
            {
                if (item == "X" || item == "0")
                {
                    gameTie = true;
                }
                else
                {
                    gameTie = false;
                    break;
                }
            }
        }

        // check if the space is open
        public static bool CheckIfOpen(int turn)
        {
            if (board[turn] == " ")
            {
                return true;
            }
            else if (turn > 8)
            {
                Console.WriteLine("enter a number between 0 and 8");
                return false;
            }
            else
            {
                Console.WriteLine("Enter another number (space is not open)");
                return false;
            }
        }


        public static bool CheckXLines()
        {
            /*
            X checking:
                - diagonals => true
                - rows => true
                - lines => true
            else:
                => false
            */ 
             
            if (board[0] == "X" && board[1] == "X" && board[2] == "X") { return true; }
            else if (board[3] == "X" && board[4] == "X" && board[5] == "X") { return true; }
            else if (board[6] == "X" && board[7] == "X" && board[8] == "X") { return true; }

            else if (board[0] == "X" && board[3] == "X" && board[6] == "X") { return true; }
            else if (board[1] == "X" && board[4] == "X" && board[7] == "X") { return true; }
            else if (board[2] == "X" && board[5] == "X" && board[8] == "X") { return true; }

            else if (board[0] == "X" && board[4] == "X" && board[8] == "X") { return true; }
            else if (board[2] == "X" && board[4] == "X" && board[6] == "X") { return true; }

            else return false;
        }

        public static bool Check0Lines()
        {
            /*
            0 checking:
                - diagonals => true
                - rows => true
                - lines => true
            else:
                => false
            */
            if (board[0] == "0" && board[1] == "0" && board[2] == "0") { return true; }
            else if (board[3] == "0" && board[4] == "0" && board[5] == "0") { return true; }
            else if (board[6] == "0" && board[7] == "0" && board[8] == "0") { return true; }

            else if (board[0] == "0" && board[3] == "0" && board[6] == "0") { return true; }
            else if (board[1] == "0" && board[4] == "0" && board[7] == "0") { return true; }
            else if (board[2] == "0" && board[5] == "0" && board[8] == "0") { return true; }

            else if (board[0] == "0" && board[4] == "0" && board[8] == "0") { return true; }
            else if (board[2] == "0" && board[4] == "0" && board[6] == "0") { return true; }

            else return false;
        }

        // re write every item in the board, for it to be open (after the game is done) 
        public static void ReWriteBoard()
        {
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = " ";
            }
        }
    }
}
