using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameCore;

namespace FrogsAndToadsCore
{
    public abstract class FrogsAndToadsPlayChooser : GamePlayer<FrogsAndToadsPosition>
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

        public override AttemptPlay<FrogsAndToadsPosition> Play(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            return
                playOptions.Count() == 0
                ? AttemptPlay<FrogsAndToadsPosition>.Failure
                : AttemptPlay<FrogsAndToadsPosition>.Success(playOptions.First());                    
        }        
    }



    public class ConsoleChooser : FrogsAndToadsPlayChooser
    {
        public ConsoleChooser(string label) 
            : base(label)
        { }

        public override AttemptPlay<FrogsAndToadsPosition> Play(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            if (playOptions.Count() == 0)
                return AttemptPlay<FrogsAndToadsPosition>.Failure;

            List<string> choices = playOptions.Select(x => x.ToString()).ToList();
            ConsoleColor resetColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

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

            return AttemptPlay<FrogsAndToadsPosition>.Success(playOptions.ToList()[result]);




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



    public class MiniMiniMaxChooser : FrogsAndToadsPlayChooser
    {
        public MiniMiniMaxChooser(string label) 
            : base(label)
        { }

        public override AttemptPlay<FrogsAndToadsPosition> Play(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            if (playOptions.Count() == 0)
                return AttemptPlay<FrogsAndToadsPosition>.Failure;

            if (playOptions.Count() == 1)
                return AttemptPlay<FrogsAndToadsPosition>.Success(playOptions.First());

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

            return AttemptPlay<FrogsAndToadsPosition>.Success(bestOption);
        }
    }



    public class EvaluatingChooser : FrogsAndToadsPlayChooser
    {
        #region private attributes
        private GamePositionEvaluator<FrogsAndToadsPosition> _evaluator;
        #endregion


        #region abstract overrides
        public override AttemptPlay<FrogsAndToadsPosition> Play(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            if (playOptions.Count() == 0)
                return AttemptPlay<FrogsAndToadsPosition>.Failure;

            if (playOptions.Count() == 1)
                return AttemptPlay<FrogsAndToadsPosition>.Success(playOptions.First());

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

            return AttemptPlay<FrogsAndToadsPosition>.Success(bestOption);
        }
        
        #endregion



        public EvaluatingChooser(string label, GamePositionEvaluator<FrogsAndToadsPosition> evaluator)
            :base(label)
        {    _evaluator = evaluator;
        
        }
        
        
    }
}