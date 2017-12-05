using FrogsAndToadsCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void MiniMiniMax()
        {
            PlayChooser chooser = new MiniMiniMaxChooser();

            GamePosition position = new GamePosition("T_TF_F");            
            PlayChoice choice = chooser.ChoosePlay(position);
            Assert.AreEqual(2, choice.Choice);

            position = new GamePosition("T_TT_F");
            choice = chooser.ChoosePlay(position);
            Assert.AreEqual(3, choice.Choice);

            position = new GamePosition("T__TF_");
            choice = chooser.ChoosePlay(position);
            Assert.AreEqual(0, choice.Choice);
        }


        [TestMethod]
        public void MiniMax()
        {
            GamePosition position;
            PlayChoice choice;
            PlayChooser chooser = new MiniMaxChooser();

            position = new GamePosition("T___");
            choice = chooser.ChoosePlay(position);
            Assert.AreEqual(0, choice.Choice);


            position = new GamePosition("T_T_");
            choice = chooser.ChoosePlay(position);
            Assert.AreEqual(0, choice.Choice);


            position = new GamePosition("T_T_F");
            choice = chooser.ChoosePlay(position);
            Assert.AreEqual(2, choice.Choice);


            position = new GamePosition("TF_TF_TF__TF");
            choice = chooser.ChoosePlay(position);
            Assert.AreEqual(0, choice.Choice);
        }
    }
}
