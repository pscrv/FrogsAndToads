using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using GameCore;
using FrogsAndToadsCore;
using System.Linq;

namespace CoreTests
{
    [TestClass]
    public class PositionEvaluatorTests
    {
        string gameString1 = "T___";
        string gameString2 = "T_F";
        string gameString3 = "TF__TF_";
        string gameString4 = "TF_T_";
        string gameString5 = "_F_TF";
        string gameString6 = "TF_T_F";

        FrogsAndToadsPosition position;
        FrogsAndToadsPositionEvaluator evaluator;
        int value;

        [TestMethod]
        public void MiniMaxEvaluator()
        {
            position = new FrogsAndToadsPosition(gameString1);
            evaluator = new MiniMaxEvaluator();
            value = evaluator.LeftEvaluation(position);
            Assert.AreEqual(2, value);

            position = new FrogsAndToadsPosition(gameString2);
            evaluator = new MiniMaxEvaluator();
            value = evaluator.LeftEvaluation(position);
            Assert.AreEqual(0, value);
            value = evaluator.LeftEvaluation(position.Reverse as FrogsAndToadsPosition);
            Assert.AreEqual(0, value);

            position = new FrogsAndToadsPosition(gameString3);
            evaluator = new MiniMaxEvaluator();
            value = evaluator.LeftEvaluation(position);
            Assert.AreEqual(2, value);

            position = new FrogsAndToadsPosition(gameString4);
            evaluator = new MiniMaxEvaluator();
            value = evaluator.LeftEvaluation(position);
            Assert.AreEqual(3, value);

            position = new FrogsAndToadsPosition(gameString5);
            evaluator = new MiniMaxEvaluator();
            value = evaluator.RightEvaluation(position);
            Assert.AreEqual(-3, value);
        }


        [TestMethod]
        public void EvaluateToadMoves()
        {
            position = new FrogsAndToadsPosition(gameString6);
            MiniMaxEvaluator evaluator = new MiniMaxEvaluator();
            var x = evaluator.EvaluateToadMoves(position);
            Assert.AreEqual(2, x.Count);
            Assert.AreEqual(-2, x.First().value);
            Assert.AreEqual(1, x.Skip(1).First().value);

            position = position.PlayMove(new FrogsAndToadsMove(0, 2));
            x = evaluator.EvaluateToadMoves(position.Reverse);
            Assert.AreEqual(2, x.Count);
            Assert.AreEqual(2, x.First().value);
            Assert.AreEqual(-1, x.Skip(1).First().value);
        }


        [TestMethod]
        public void GetSubPositions()
        {
            List<FrogsAndToadsPosition> subPositions;

            position = new FrogsAndToadsPosition("TF__TF_");
            subPositions = position.GetSubPositions();
            Assert.AreEqual("< T F _ _ T F _ >", subPositions.First().ToString());


            position = new FrogsAndToadsPosition("T__TTFF___F");
            subPositions = position.GetSubPositions();
            Assert.AreEqual(2, subPositions.Count);
            Assert.AreEqual("< T _ _ >", subPositions[0].ToString());
            Assert.AreEqual("< _ _ _ F >", subPositions[1].ToString());

            position = new FrogsAndToadsPosition("TFTFF__TTFF_TF_T___TTFTFFFFTTF_FT");
            subPositions = position.GetSubPositions();
            Assert.AreEqual(3, subPositions.Count);
            Assert.AreEqual("< _ _ >", subPositions[0].ToString());
            Assert.AreEqual("< _ T F _ T _ _ _ >", subPositions[1].ToString());
            Assert.AreEqual("< T T F _ F T >", subPositions[2].ToString());
        }

        [TestMethod]
        public void EvaluateEndPosition()
        {
            MiniMaxEvaluator evaluator = new MiniMaxEvaluator();
            position = new FrogsAndToadsPosition("TT__");
            Assert.AreEqual(4, evaluator.EvaluateEndPositionForToads(position));

            position = new FrogsAndToadsPosition("__F_F______");
            Assert.AreEqual(-5, evaluator.EvaluateEndPositionForFrogs(position));

            position = new FrogsAndToadsPosition("TFTFF__TTFF___T_T___TTFTFFFF_TTT___");
            int result = evaluator.EvaluateEndPositionForToads(position);
            Assert.AreEqual(0 + 7 + 9, result);
        }
        
    }
}
