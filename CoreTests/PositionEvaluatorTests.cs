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

        FrogsAndToadsPosition position;
        GamePositionEvaluator evaluator;
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

        }


        [TestMethod]
        public void GetSubPositions()
        {
            List<FrogsAndToadsPosition> subPositions;

            position = new FrogsAndToadsPosition(gameString3);
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
            position = new FrogsAndToadsPosition(gameString3);            
            Assert.AreEqual(2, evaluator.EvaluateEndPosition(position));

            position = new FrogsAndToadsPosition("T__TTFF___F");
            Assert.AreEqual(-1, evaluator.EvaluateEndPosition(position));

            position = new FrogsAndToadsPosition("TFTFF__TTFF_TF_T___TTFTFFFFTTF_FT");
            Assert.AreEqual(0 + (8 - 2) + (6 - 5), evaluator.EvaluateEndPosition(position));
        }
    }
}
