using System.Collections.Generic;
using System.Linq;
using FrogsAndToadsCore;
using GameCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class PlayerTests
    {
        PlayChooser chooser;
        AttemptPlay result;
        FrogsAndToadsPosition position;

        //IEnumerable<GamePosition> options 
        //    => position.GetPossibleToadMoves()
        //    .Select(x => position.MovePiece(x));

        IEnumerable<FrogsAndToadsPosition> options
            => position.GetPossibleToadMoves()
            .Select(x => position.MovePiece(x));



        [TestMethod]
        public void MiniMiniMax()
        {
            chooser = new MiniMiniMaxChooser("");
            position = new FrogsAndToadsPosition("T_TF_F");
            result = chooser.ChoosePlay(options);
            Assert.AreEqual(position.MovePiece(2).ToString(), result.Value.ToString());

            position = new FrogsAndToadsPosition("T_TT_F");
            result = chooser.ChoosePlay(options);
            Assert.AreEqual(position.MovePiece(3).ToString(), result.Value.ToString());

            position = new FrogsAndToadsPosition("T__TF_");
            result = chooser.ChoosePlay(options);
            Assert.AreEqual(position.MovePiece(0).ToString(), result.Value.ToString());
        }


        [TestMethod]
        public void MiniMax()
        {
            chooser = new EvaluatingChooser("", new MiniMaxEvaluator());

            position = new FrogsAndToadsPosition("T___");
            result = chooser.ChoosePlay(options);
            Assert.AreEqual(position.MovePiece(0).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("T_T_");
            result = chooser.ChoosePlay(options);
            Assert.AreEqual(position.MovePiece(0).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("T_T_F");
            result = chooser.ChoosePlay(options);
            Assert.AreEqual(position.MovePiece(2).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("TF_TF_TF__TF");
            result = chooser.ChoosePlay(options);
            Assert.AreEqual(position.MovePiece(0).ToString(), result.Value.ToString());
       

            position = new FrogsAndToadsPosition("T___");
            result = chooser.ChoosePlay(options);
            Assert.AreEqual(position.MovePiece(0).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("T_T_");
            result = chooser.ChoosePlay(options);  
            Assert.AreEqual(position.MovePiece(0).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("T_T_F");
            result = chooser.ChoosePlay(options);
            Assert.AreEqual(position.MovePiece(2).ToString(), result.Value.ToString());


            position = new FrogsAndToadsPosition("TF_TF_TF__TF");
            result = chooser.ChoosePlay(options);
            Assert.AreEqual(position.MovePiece(0).ToString(), result.Value.ToString());
        }
    }
}
