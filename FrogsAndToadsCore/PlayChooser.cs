﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameCore;

namespace FrogsAndToadsCore
{
    public abstract class PlayChooser : GamePlayer
    {
        #region abstract
        internal abstract AttemptPlay ChoosePlay(IEnumerable<FrogsAndToadsPosition> playOptions);
        #endregion



        #region construction
        public PlayChooser(string label) 
            : base(label)
        { }
        #endregion
        


        #region GamePlayer
        //public AttemptPlay ChoosePlay(IEnumerable<GamePosition> playOptions)
        //{
        //    IEnumerable<FrogsAndToadsPosition> options = 
        //        playOptions.Select(x => x as FrogsAndToadsPosition);
        //    return ChoosePlay(options);
        //}

        public override AttemptPlay Play(IEnumerable<GamePosition> playOptions)
        {
            IEnumerable<FrogsAndToadsPosition> options =
                playOptions.Select(x => x as FrogsAndToadsPosition);
            return ChoosePlay(options);
        }
        #endregion
    }



    public class TrivialChooser : PlayChooser
    {
        public TrivialChooser(string label) 
            : base(label)
        { }

        internal override AttemptPlay ChoosePlay(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            return
                playOptions.Count() == 0
                ? AttemptPlay.Failure
                : AttemptPlay.Success(playOptions.First());                    
        }        
    }



    public class ConsoleChooser : PlayChooser
    {
        public ConsoleChooser(string label) 
            : base(label)
        { }

        internal override AttemptPlay ChoosePlay(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            if (playOptions.Count() == 0)
                return AttemptPlay.Failure;

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

            return AttemptPlay.Success(playOptions.ToList()[result]);




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



    public class MiniMiniMaxChooser : PlayChooser
    {
        public MiniMiniMaxChooser(string label) 
            : base(label)
        { }

        internal override AttemptPlay ChoosePlay(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            if (playOptions.Count() == 0)
                return AttemptPlay.Failure;

            if (playOptions.Count() == 1)
                return AttemptPlay.Success(playOptions.First());

            GamePosition bestOption = playOptions.First();
            FrogsAndToadsPosition currentOption;
            int maximum = int.MinValue;
            foreach (GamePosition option in playOptions)
            {
                currentOption = option as FrogsAndToadsPosition;

                List<int> possibleResponses = currentOption.GetPossibleFrogMoves();

                int minimum = int.MaxValue;
                foreach (int response in possibleResponses)
                {
                    int reResponseCount;
                    reResponseCount = currentOption.MovePiece(response).GetPossibleToadMoves().Count;

                    if (reResponseCount < minimum)
                        minimum = reResponseCount;
                }

                if (minimum > maximum)
                {
                    maximum = minimum;
                    bestOption = option;
                }

            }

            return AttemptPlay.Success(bestOption);
        }
    }



    public class EvaluatingChooser : PlayChooser
    {
        #region private attributes
        private GamePositionEvaluator _evaluator;
        #endregion


        #region abstract overrides
        internal override AttemptPlay ChoosePlay(IEnumerable<FrogsAndToadsPosition> playOptions)
        {
            if (playOptions.Count() == 0)
                return AttemptPlay.Failure;

            if (playOptions.Count() == 1)
                return AttemptPlay.Success(playOptions.First());

            int optionValue;
            int bestValue = int.MinValue;
            GamePosition bestOption = null;
            foreach (GamePosition option in playOptions)
            {
                optionValue = _evaluator.FrogEvaluation(option as FrogsAndToadsPosition);
                if (optionValue > bestValue)
                {
                    bestValue = optionValue;
                    bestOption = option;
                }
            }            

            return AttemptPlay.Success(bestOption);
        }
        
        #endregion



        public EvaluatingChooser(string label, GamePositionEvaluator evaluator)
            :base(label)
        {    _evaluator = evaluator;
        
        }
        
        
    }
}