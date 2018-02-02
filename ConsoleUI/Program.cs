using System;

using FrogsAndToadsCore;
using GameCore;
using NoughtsAndCrossesCore;

namespace ConsoleUI
{
    class Program
    {
        const string __GameString__ = "TF_TF_TF__TF";
        //const string __GameString__ = "TF__TF_TF__TF";
        //const string __GameString__ = "TF__TF_";
        //const string __GameString__ = "T_T_F";
        //const string __GameString__ = "TF_T_F";



        static void Main(string[] args)
        {
            //PlayFrogsAndToads();
            PlayNoughtsAndCrosses();
        }


        static void PlayFrogsAndToads()
        {
            GamePlayer<FrogsAndToadsPosition> _toadChooser = new FrogsAndToadsCore.ConsolePlayer("Toads");
            GamePlayer<FrogsAndToadsPosition> _frogsChooser = new FrogsAndToadsCore.EvaluatingPlayer("Frogs", new MiniMaxEvaluator());
            FrogsAndToadsGame game = new FrogsAndToadsGame(
                _toadChooser,
                _frogsChooser,
                __GameString__
                );
            PlayInTurns(game);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Game over with position {game.PositionString}.");
            Console.WriteLine($"The winner was {game.Winner}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("History:");
            foreach (string positionString in game.StringHistory)
            {
                Console.WriteLine($"    {positionString}");
            }




            Console.ReadLine();

        }


        static void PlayInTurns(FrogsAndToadsGame game)
        {
            while (! game.GameIsOver)
            {
                Console.WriteLine();
                Console.WriteLine($"   current position is {game.Position}");
                Console.WriteLine("      Press <Enter>");
                Console.ReadLine();
                game.PlayRound();
            }
        }



        static void PlayNoughtsAndCrosses()
        {
            GamePlayer<NoughtsAndCrossesPosition> _crossesChooser = new NoughtsAndCrossesCore.ConsolePlayer("Crosses");
            GamePlayer<NoughtsAndCrossesPosition> _noughtsChooser = new NoughtsAndCrossesCore.TrivialPlayer("Noughts");
            NoughtsAndCrossesGame game = new NoughtsAndCrossesGame(
                _crossesChooser,
                _noughtsChooser,
                3 );
            PlayInTurns(game);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Game over with position {game.PositionString}.");
            Console.WriteLine($"The winner was {game.Winner}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("History:");
            foreach (string positionString in game.StringHistory)
            {
                Console.WriteLine($"    {positionString}");
            }

            Console.ReadLine();
        }

        

        static void PlayInTurns(NoughtsAndCrossesGame game)
        {
            while (!game.GameIsOver)
            {
                Console.WriteLine();
                Console.WriteLine($"   current position is \n{game.Position}");
                Console.WriteLine("      Press <Enter>");
                Console.ReadLine();
                game.PlayRound();
            }
        }

    }
}
