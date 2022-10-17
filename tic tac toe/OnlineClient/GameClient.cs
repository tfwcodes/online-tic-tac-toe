using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClientTicTacToe;

namespace ClientTicTacToe
{
    public static class GameClient
    {

        // main vars
        public static string[] board { get; set; } = { " ", " ", " ", " ", " ", " ", " ", " ", " " };
        public static bool gameTie { get; set; }


        // render the board with the current turn's
        public static void RenderBoard()
        {
            Console.WriteLine($"| {board[0]} | {board[1]} | {board[2]} | \n");
            Console.WriteLine($"| {board[3]} | {board[4]} | {board[5]} | \n");
            Console.WriteLine($"| {board[6]} | {board[7]} | {board[8]} | \n");
        }

        // render the board at the beginning so that the players know the positions
        public static void RenderBeginningBoard()
        {
            Console.WriteLine("| 0 | 1 | 2|");
            Console.WriteLine("| 3 | 4 | 5|");
            Console.WriteLine("| 6 | 7 | 8|");
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

        public static void Engine(this Stream stream)
        {
            RenderBeginningBoard();
            while (true)
            {

                Console.WriteLine("Server's turn...");

                var turnServer = Protocol.ReadString(stream); // read the turn
                board[Convert.ToInt32(turnServer)] = "X";
                RenderBoard();
                checkTie(); // check for tine


                // check if x won
                if (CheckXLines())
                {
                    Console.WriteLine("The server won");
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

                else
                {

                ReWrite: // the ReWrite: is so that if the move have already been put the player will need to try again

                    Console.Write($"$~ Enter a position from 0-8: ");
                    int turn = Convert.ToInt32(Console.ReadLine());
                    if (CheckIfOpen(turn)) // check if the space is open
                    {
                        Protocol.WriteString(stream, Convert.ToString(turn)); // send the turn to the server
                        board[turn] = "0";
                        RenderBoard();
                        checkTie(); // check for a tie

                        if (Check0Lines()) // check the lines for 0 
                        {
                            Console.WriteLine("You won");
                            Thread.Sleep(5000);
                            ReWriteBoard();
                            break;
                        }
                        else if (gameTie) // check for the gameTie var
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
                }



            }

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
