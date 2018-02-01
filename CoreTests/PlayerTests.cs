using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameCore;
using FrogsAndToadsCore;
using Monads;

namespace CoreTests1
{
    [TestClass]
    public class PlayerTests
    {
        GamePlayer<FrogsAndToadsPosition> player;
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
            MiniMiniMaxPlayer mmplayer = new MiniMiniMaxPlayer("");
            position = new FrogsAndToadsPosition("T_TF_F");

            result = mmplayer.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(2, 4);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());

            position = new FrogsAndToadsPosition("T_TT_F");
            result = mmplayer.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(3, 4);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());

            position = new FrogsAndToadsPosition("T__TF_");
            result = mmplayer.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(0, 1);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());
        }


        [TestMethod]
        public void MiniMax()
        {
            player = new EvaluatingPlayer("", new MiniMaxEvaluator());

            position = new FrogsAndToadsPosition("T___");
            result = player.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(0, 1);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("T_T_");
            result = player.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(0, 1);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("T_T_F");
            result = player.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(2, 3);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("T_T___FF");
            result = player.PlayLeft(_options);

            
            position = new FrogsAndToadsPosition("TF_TF_TF__TF");
            result = player.PlayLeft(_options);
            correctMove = new FrogsAndToadsMove(0, 2);
            Assert.AreEqual(position.PlayMove(correctMove).ToString(), result.Value.ToString());
            
        }
    }
}
