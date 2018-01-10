using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameCore;
using FrogsAndToadsCore;
using Monads;

namespace CoreTests
{
    [TestClass]
    public class PlayerTests
    {
        FrogsAndToadsPlayer chooser;
        Maybe<FrogsAndToadsPosition> result;
        FrogsAndToadsPosition position;
        FrogsAndToadsMove correctMove;
        

        IEnumerable<FrogsAndToadsPosition> _options
        {
            get
            {
                return position.GetPossibleToadMoves()
                    .Select(x => position.PlayMove(x));
            }
        }



        [TestMethod]
        public void MiniMiniMax()
        {
            chooser = new MiniMiniMaxPlayer("");
            position = new FrogsAndToadsPosition("T_TF_F");

            result = chooser.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(2, 4);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());

            position = new FrogsAndToadsPosition("T_TT_F");
            result = chooser.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(3, 4);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());

            position = new FrogsAndToadsPosition("T__TF_");
            result = chooser.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(0, 1);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());
        }


        [TestMethod]
        public void MiniMax()
        {
            chooser = new EvaluatingPlayer("", new MiniMaxEvaluator());

            position = new FrogsAndToadsPosition("T___");
            result = chooser.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(0, 1);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("T_T_");
            result = chooser.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(0, 1);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("T_T_F");
            result = chooser.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(2, 3);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("TF_TF_TF__TF");
            result = chooser.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(0, 2);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());
       

            position = new FrogsAndToadsPosition("T___");
            result = chooser.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(0, 1);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("T_T_");
            result = chooser.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(0, 1);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("T_T_F");
            result = chooser.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(2, 3);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("TF_TF_TF__TF");
            result = chooser.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(0, 2);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());
        }
    }
}
