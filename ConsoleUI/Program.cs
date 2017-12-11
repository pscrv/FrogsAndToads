using System;
using FrogsAndToadsCore;

namespace ConsoleUI
{
    class Program
    {
        //const string __GameString__ = "TF__TF_TF__TF";
        const string __GameString__ = "TF__TF_";


        static void Main(string[] args)
        {
            FrogsAndToadGame game = new FrogsAndToadGame(
                __GameString__, 
                new ConsoleChooser(), 
                new EvaluatingChooser(
                    new MiniMaxEvaluator()));
            PlayInTurns(game);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Game over with position {game.CurrentPosition}.");
            Console.WriteLine($"The winner was {game.Winner}");

            Console.ReadLine();

        }


        static void PlayInTurns(FrogsAndToadGame game)
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
