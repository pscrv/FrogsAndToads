using System;
using FrogsAndToadsCore;

namespace ConsoleUI
{
    class Program
    {

        static void Main(string[] args)
        {
            Game game = new Game("TF_TF_TF_TF", new ConsolePlayer(), new MiniMiniMaxPlayer());
            PlayInTurns(game);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Game over with position {game.CurrentPosition}.");
            Console.WriteLine($"The winner was {game.Winner}");

            Console.ReadLine();

        }


        static void PlayWholeGame(Game game)
        {
            game.Play();
        }


        static void PlayInTurns(Game game)
        {
            while (game.GameIsRunning)
            {
                game.PlayOneMove();

                if (game.GameIsRunning)
                {
                    Console.WriteLine();
                    Console.WriteLine($"   {game.ActivePlayer} moved to {game.CurrentPosition}");
                    Console.WriteLine("      Press <Enter>");
                    Console.ReadLine();
                }
            }
        }

    }
}
