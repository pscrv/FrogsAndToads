using System;
using FrogsAndToadsCore;

namespace ConsoleUI
{
    class Program
    {

        static void Main(string[] args)
        {
            Game game = new Game("TF_TF_TF__TF", new ConsoleChooser(), new MiniMiniMaxChooser());
            PlayInTurns(game);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Game over with position {game.CurrentPosition}.");
            Console.WriteLine($"The winner was {game.Winner}");

            Console.ReadLine();

        }


        static void PlayInTurns(Game game)
        {
            foreach (GamePosition position in game.GameMoves())
            {
                Console.WriteLine();
                Console.WriteLine($"   {game.ActivePlayer} moved to {position}");
                Console.WriteLine("      Press <Enter>");
                Console.ReadLine();
            }            
        }
    }
}
