using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrogsAndToadsCore
{
    public abstract class PlayChooser
    {
        #region abstract
        internal abstract PlayChoice ChoosePlay(GamePosition position);
        #endregion    
    }



    public class TrivialChooser : PlayChooser
    {
        internal override PlayChoice ChoosePlay(GamePosition position)
        {
            List<int> possiblePlays = position.GetPossibleToadMoves();
            if (possiblePlays.Count == 0)
                return PlayChoice.NoChoice();
            return PlayChoice.ChoiceMade(possiblePlays.First());
        }
    }



    public class ConsoleChooser : PlayChooser
    {        
        internal override PlayChoice ChoosePlay(GamePosition position)
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



    public class MiniMiniMaxChooser : PlayChooser
    {
        internal override PlayChoice ChoosePlay(GamePosition position)
        {
            List<int> possiblePlays = position.GetPossibleToadMoves();

            if (possiblePlays.Count == 0)
                return PlayChoice.NoChoice();

            if (possiblePlays.Count == 1)
                return PlayChoice.ChoiceMade(possiblePlays.First());
            
            int bestPlay = possiblePlays.First();
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



    public class MiniMaxChooser : PlayChooser
    {
        internal override PlayChoice ChoosePlay(GamePosition position)
        {
            GamePosition resultingPosition;
            List<int> possibleToadMoves = position.GetPossibleToadMoves();

            if (possibleToadMoves.Count == 0)
                return PlayChoice.NoChoice();

            if (possibleToadMoves.Count == 1)
                return PlayChoice.ChoiceMade(possibleToadMoves.First());

            int bestmove = int.MinValue;
            int bestvalue = int.MinValue;
            int currentvalue;
            foreach (int move in possibleToadMoves)
            {
                resultingPosition = position.MovePiece(move);
                currentvalue = _evaluatePositionForFrog(resultingPosition, 0, int.MinValue, int.MaxValue);
                if (currentvalue > bestvalue)
                {
                    bestvalue = currentvalue;
                    bestmove = move;
                }
            }

            return PlayChoice.ChoiceMade(bestmove);
        }
        


        private int _evaluatePositionForToad(GamePosition position, int depth, int bestToad, int bestFrog)
        {
            GamePosition resultingPosition;
            List<int> possibleMoves = position.GetPossibleToadMoves();

            if (possibleMoves.Count == 0)
            {
                return _evaluateEndPositionForFrogs(position);
            }


            int bestvalue = int.MinValue;
            foreach (int move in possibleMoves)
            {
                resultingPosition = position.MovePiece(move);
                bestvalue =
                    Math.Max(
                        bestvalue,
                        _evaluatePositionForFrog(resultingPosition, depth + 1, bestToad, bestFrog)
                        );
                bestToad = Math.Max(bestToad, bestvalue);
                if (bestToad > bestFrog)
                    break;
            }

            return bestvalue;
        }


        private int _evaluatePositionForFrog(GamePosition position, int depth, int bestToad, int bestFrog)
        {
            GamePosition resultingPosition;
            List<int>possibleMoves = position.GetPossibleFrogMoves();

            if (possibleMoves.Count == 0)
            {
                return _evaluateEndPositionForToads(position);
            }

            int bestvalue = int.MaxValue;
            foreach (int move in possibleMoves)
            {
                resultingPosition = position.MovePiece(move);
                bestvalue =
                    Math.Min(
                        bestvalue,
                        _evaluatePositionForToad(resultingPosition, depth + 1, bestToad, bestFrog)
                        );
                bestFrog = Math.Min(bestFrog, bestvalue);
                if (bestToad > bestFrog)
                    break;
            }

            return bestvalue;
        }

        

        private int _evaluateEndPositionForToads(GamePosition position)
        {
            return 1 + position.GetPossibleToadMoves().Count;
        }

        private int _evaluateEndPositionForFrogs(GamePosition position)
        {
            return -1 - position.GetPossibleFrogMoves().Count;
        }  
    }
}