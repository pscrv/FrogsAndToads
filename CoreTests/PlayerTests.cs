using FrogsAndToadsCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void MiniMax()
        {
            Player minimaxPlayer = new MiniMiniMaxPlayer();

            GamePosition position = new GamePosition("T_TF_F");            
            PlayChoice choice = minimaxPlayer.ChooseToadPlay(position);
            Assert.AreEqual(2, choice.Choice);

            position = new GamePosition("T_TT_F");
            choice = minimaxPlayer.ChooseToadPlay(position);
            Assert.AreEqual(3, choice.Choice);

            position = new GamePosition("T__TF_");
            choice = minimaxPlayer.ChooseToadPlay(position);
            Assert.AreEqual(0, choice.Choice);
        }
    }
}
