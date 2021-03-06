﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GameCore
{
    public class EvaluationRecord
    {
        public readonly int Value;
        public readonly bool IsComplete;
        public readonly ReadOnlyCollection<GamePosition> EvaluatedPositions;


        public EvaluationRecord(int value, IList<GamePosition> evaluatedPositions)
        {
            Value = value;
            IsComplete = false;
            EvaluatedPositions = new ReadOnlyCollection<GamePosition>(evaluatedPositions);
        }

        public EvaluationRecord(int value)
        {
            Value = value;
            IsComplete = true;
            EvaluatedPositions = null;
        }
    }
}
