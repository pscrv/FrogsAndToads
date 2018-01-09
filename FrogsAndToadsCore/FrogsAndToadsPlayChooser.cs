using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameCore;
using Monads;

namespace FrogsAndToadsCore
{
    public abstract class FrogsAndToadsPlayChooser : PartisanGamePlayer<FrogsAndToadsPosition>
    {
        #region construction
        public FrogsAndToadsPlayChooser(string label) 
            : base(label)
        { }
        #endregion
    }



    public class TrivialChooser : FrogsAndToadsPlayChooser
    {
        public TrivialChooser(string label) 
            : base(label)
        { }

        public override Maybe<FrogsAndToadsPosition> PlayLeft(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            return
                playOptions.Count() == 0
                ? Maybe<FrogsAndToadsPosition>.Nothing()
                : playOptions.First().ToMaybe();                    
        }

        public override Maybe<FrogsAndToadsPosition> PlayRight(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            return PlayLeft(playOptions);
        }
    }



    public class ConsoleChooser : FrogsAndToadsPlayChooser
    {
        public ConsoleChooser(string label) 
            : base(label)
        { }

        public override Maybe<FrogsAndToadsPosition> PlayLeft(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            const ConsoleColor choiceColor = ConsoleColor.Yellow;

            if (playOptions.Count() == 0)
                return Maybe<FrogsAndToadsPosition>.Nothing();

            List<string> choices = playOptions.Select(x => x.ToString()).ToList();
            ConsoleColor resetColour = Console.ForegroundColor;
            Console.ForegroundColor = choiceColor;

            string input;
            int result = -1;
            while (result < 0 || result >= choices.Count)
            {
                Console.WriteLine($"Please choose a move from: ");
                for (int i = 0; i < choices.Count; i++)
                {
                    Console.WriteLine($"    {i}: {choices[i]}");
                }

                input = Console.ReadLine();
                if (int.TryParse(input, out int res))
                    result = res;
            }

            Console.ForegroundColor = resetColour;

            return playOptions.ToList()[result].ToMaybe();
        }

        public override Maybe<FrogsAndToadsPosition> PlayRight(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            return PlayLeft(playOptions);
        }

        private string ListToString<T>(List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            for (int i = 0; i < list.Count - 1; i++)
            {
                sb.Append($"{list[i].ToString()}, ");
            }
            sb.Append(list[list.Count - 1]);
            sb.Append(']');
            return sb.ToString();
        }               
    }



    public abstract class SymmetricChooser : FrogsAndToadsPlayChooser
    {
        public SymmetricChooser(string label) 
            : base(label)
        { }


        public override Maybe<FrogsAndToadsPosition> PlayRight(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            var reversedOptions =
                from x in playOptions
                select x.Reverse();

            var bestOption = PlayLeft(reversedOptions);

            return
                from x in bestOption
                select x.Reverse();
        }
    }


    public class MiniMiniMaxChooser : SymmetricChooser
    {
        public MiniMiniMaxChooser(string label) 
            : base(label)
        { }

        public override Maybe<FrogsAndToadsPosition> PlayLeft(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            if (playOptions.Count() == 0)
                return Maybe<FrogsAndToadsPosition>.Nothing();

            if (playOptions.Count() == 1)
                return playOptions.First().ToMaybe();

            FrogsAndToadsPosition bestOption = playOptions.First();
            FrogsAndToadsPosition currentOption;
            int maximum = int.MinValue;
            foreach (FrogsAndToadsPosition option in playOptions)
            {
                currentOption = option;
                
                List<FrogsAndToadsMove> possibleResponses = currentOption.GetPossibleFrogMoves();

                int minimum = int.MaxValue;
                foreach (FrogsAndToadsMove response in possibleResponses)
                {
                    int reResponseCount = 
                        currentOption
                        .PlayMove(response)
                        .GetPossibleToadMoves()
                        .Count;

                    if (reResponseCount < minimum)
                        minimum = reResponseCount;
                }

                if (minimum > maximum)
                {
                    maximum = minimum;
                    bestOption = option;
                }

            }

            return bestOption.ToMaybe();
        }
        
    }



    public class EvaluatingChooser : SymmetricChooser
    {
        public EvaluatingChooser(string label, PartisanGamePositionEvaluator<FrogsAndToadsPosition> evaluator)
            :base(label)
        {
            _evaluator = evaluator;        
        }

        #region private attributes
        private PartisanGamePositionEvaluator<FrogsAndToadsPosition> _evaluator;
        #endregion


        #region abstract overrides
        public override Maybe<FrogsAndToadsPosition> PlayLeft(IEnumerable<FrogsAndToadsPosition> playOptions)
        {              

            if (playOptions.Count() == 0)
                return Maybe<FrogsAndToadsPosition>.Nothing();

            if (playOptions.Count() == 1)
                return playOptions.First().ToMaybe();

            int optionValue;
            int bestValue = int.MinValue;
            FrogsAndToadsPosition bestOption = null;
            foreach (FrogsAndToadsPosition option in playOptions)
            {
                optionValue = _evaluator.RightEvaluation(option);
                if (optionValue > bestValue)
                {
                    bestValue = optionValue;
                    bestOption = option;
                }
            }            

            return bestOption.ToMaybe();
        }
        #endregion
    }
}