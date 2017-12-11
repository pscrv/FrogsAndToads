using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FrogsAndToadsCore;
using System.Collections.Generic;

namespace CoreTests
{
    [TestClass]
    public class PositionEvaluatorTests
    {
        string gameString1 = "T___";
        string gameString2 = "T_F";
        string gameString3 = "TF__TF_";

        GamePosition position;
        PositionEvaluator evaluator;
        int value;
        Dictionary<int, int> values;

        [TestMethod]
        public void MiniMaxEvaluator()
        {
            position = new GamePosition(gameString1);
            evaluator = new MiniMaxEvaluator();
            value = evaluator.ToadEvaluation(position);
            Assert.AreEqual(1, value);

            position = new GamePosition(gameString2);
            evaluator = new MiniMaxEvaluator();
            value = evaluator.ToadEvaluation(position);
            Assert.AreEqual(0, value);
            value = evaluator.ToadEvaluation(position.Reverse());
            Assert.AreEqual(0, value);
            
            position = new GamePosition(gameString3);
            evaluator = new MiniMaxEvaluator();
            value = evaluator.ToadEvaluation(position);
            Assert.AreEqual(1, value);
                        

            position = new GamePosition(gameString1);
            evaluator = new MiniMaxEvaluator();
            values = evaluator.ToadMoveEvaluations(position);
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(1, values[0]);

            position = new GamePosition(gameString2);
            evaluator = new MiniMaxEvaluator();
            values = evaluator.ToadMoveEvaluations(position);
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(0, values[0]);


            position = new GamePosition(gameString3);
            evaluator = new MiniMaxEvaluator();
            values = evaluator.ToadMoveEvaluations(position);
            Assert.AreEqual(1, values[0]);
            Assert.AreEqual(-1, values[4]);

        }
    }
}
