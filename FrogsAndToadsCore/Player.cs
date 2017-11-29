using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrogsAndToadsCore
{
    public abstract class Player
    {
        #region enum
        protected enum PieceType { Toad, Frog }
        #endregion


        #region abstract
        protected abstract PlayChoice _chooseToadPlay(GamePosition position);
        #endregion
        


        #region internal methods
        internal PlayChoice ChooseToadPlay(GamePosition position)
        {
            return _chooseToadPlay(position);
        }

        internal PlayChoice ChooseFrogPlay(GamePosition position)
        {
            GamePosition reversePosition = position.Reverse();
            PlayChoice choice = _chooseToadPlay(reversePosition);

            return choice.NoChoiceMade
                ? choice
                : PlayChoice.ChoiceMade(position.Length - 1 - choice.Choice);
        }
        #endregion
    }



    public class TrivialPlayer : Player
    {
        protected override PlayChoice _chooseToadPlay(GamePosition position)
        {
            List<int> possiblePlays = position.GetPossibleToadMoves();
            if (possiblePlays.Count == 0)
                return PlayChoice.NoChoice();
            return PlayChoice.ChoiceMade(possiblePlays[0]);
        }
    }



    public class ConsolePlayer : Player
    {        
        protected override PlayChoice _chooseToadPlay(GamePosition position)
        {
            List<int> possiblePlays = position.GetPossibleToadMoves();

            if (possiblePlays.Count == 0)
                return PlayChoice.NoChoice();
            
            List<string> choices = possiblePlays.Select(x => x.ToString()).ToList();

            ConsoleColor resetColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"The position is: {position}");

            string result = "";
            while (!choices.Contains(result))
            {
                Console.WriteLine($"Please choose a move from the list {ListToString<string>(choices)}");
                result = Console.ReadLine();
            }
            Console.ForegroundColor = resetColour;
            return PlayChoice.ChoiceMade(int.Parse(result));
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



    public class MiniMiniMaxPlayer : Player
    {
        protected override PlayChoice _chooseToadPlay(GamePosition position)
        {
            List<int> possiblePlays = position.GetPossibleToadMoves();

            if (possiblePlays.Count == 0)
                return PlayChoice.NoChoice();

            if (possiblePlays.Count == 1)
                return PlayChoice.ChoiceMade(possiblePlays[0]);
            
            int bestPlay = possiblePlays[0];
            int maximum = int.MinValue;
            foreach (int play in possiblePlays)
            {
                GamePosition resultingPosition = position.MovePiece(play);

                List<int> possibleResponses = resultingPosition.GetPossibleFrogMoves();
                            
                int minimum = int.MaxValue;
                foreach (int response in possibleResponses)
                {
                    int reResponseCount;
                    reResponseCount = resultingPosition.MovePiece(response).GetPossibleToadMoves().Count;

                    if (reResponseCount < minimum)
                        minimum = reResponseCount;
                }

                if (minimum > maximum)
                {
                    maximum = minimum;
                    bestPlay = play;
                }

            }

            return PlayChoice.ChoiceMade(bestPlay);
        }                
    }
}