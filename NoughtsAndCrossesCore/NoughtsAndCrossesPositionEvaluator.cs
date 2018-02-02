using System;
using GameCore;

namespace NoughtsAndCrossesCore
{
    class MiniMaxEvaluator : MiniMaxEvaluator<NoughtsAndCrossesPosition>
    {
        public override int EvaluateEndPositionForLeft(NoughtsAndCrossesPosition position)
        {
            return position.FreeSpaceCount;
        }

        public override int EvaluateEndPositionForRight(NoughtsAndCrossesPosition position)
        {
            return EvaluateEndPositionForLeft(position);
        }
    }
}
