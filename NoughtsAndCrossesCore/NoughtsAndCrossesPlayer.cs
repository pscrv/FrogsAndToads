using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameCore;
using Monads;

namespace NoughtsAndCrossesCore
{
    public abstract class NoughtsAndCrossesPlayer : GamePlayer<NoughtsAndCrossesPosition>
    {
        #region construction
        public NoughtsAndCrossesPlayer(string label) 
            : base(label)
        { }
        #endregion
    }



    public class TrivialPlayer : NoughtsAndCrossesPlayer
    {
        public TrivialPlayer(string label) 
            : base(label)
        { }

        public override Maybe<NoughtsAndCrossesPosition> PlayLeft(IEnumerable<NoughtsAndCrossesPosition> playOptions)
        {
            return
                playOptions.Count() == 0
                ? Maybe<NoughtsAndCrossesPosition>.Nothing()
                : playOptions.First().ToMaybe();                    
        }

        public override Maybe<NoughtsAndCrossesPosition> PlayRight(IEnumerable<NoughtsAndCrossesPosition> playOptions)
        {
            return PlayLeft(playOptions);
        }
    }



    public class ConsolePlayer : NoughtsAndCrossesPlayer
    {
        public ConsolePlayer(string label) 
            : base(label)
        { }

        public override Maybe<NoughtsAndCrossesPosition> PlayLeft(IEnumerable<NoughtsAndCrossesPosition> playOptions)
        {
            const ConsoleColor choiceColor = ConsoleColor.Yellow;

            if (playOptions.Count() == 0)
                return Maybe<NoughtsAndCrossesPosition>.Nothing();

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

        public override Maybe<NoughtsAndCrossesPosition> PlayRight(IEnumerable<NoughtsAndCrossesPosition> playOptions)
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
}