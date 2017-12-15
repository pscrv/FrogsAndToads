﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using GameCore;
using FrogsAndToadsCore;

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
            value = evaluator.ToadEvaluation(position);
            Assert.AreEqual(1, value);

            position = new FrogsAndToadsPosition(gameString2);
            evaluator = new MiniMaxEvaluator();
            value = evaluator.ToadEvaluation(position);
            Assert.AreEqual(0, value);
            value = evaluator.ToadEvaluation(position.Reverse as FrogsAndToadsPosition);
            Assert.AreEqual(0, value);
            
            position = new FrogsAndToadsPosition(gameString3);
            evaluator = new MiniMaxEvaluator();
            value = evaluator.ToadEvaluation(position);
            Assert.AreEqual(1, value);                       
            
        }
    }
}
