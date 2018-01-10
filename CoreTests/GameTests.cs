using FrogsAndToadsCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CoreTests
{
    [TestClass]
    public class GameTests
    {
        FrogsAndToadsPlayer toadPlayer = new TrivialPlayer("Toads");
        FrogsAndToadsPlayer frogPlayer = new TrivialPlayer("Frogs");

        
        [TestMethod]
        public void Play()
        {
            FrogsAndToadsGame game = new FrogsAndToadsGame(toadPlayer, frogPlayer, "T_F"); 
            game.Play();

            Assert.AreEqual(4, game.History.Count);
            Assert.AreEqual("< T _ F >", game.History[0].ToString());
            Assert.AreEqual("< _ T F >", game.History[1].ToString());
            Assert.AreEqual("< F T _ >", game.History[2].ToString());
            Assert.AreEqual("< F _ T >", game.History[3].ToString());
        }

        [TestMethod]
        public void PlayOneMove()
        {
            FrogsAndToadsGame game = new FrogsAndToadsGame(toadPlayer, frogPlayer, "T_F");
            Assert.AreEqual("< T _ F >", game.PositionString);

            game.PlayLeft();
            Assert.AreEqual("< _ T F >", game.PositionString);

            game.PlayRight();
            Assert.AreEqual("< F T _ >", game.PositionString);

            game.PlayLeft();
            Assert.AreEqual("< F _ T >", game.PositionString);
        }

        
    }
}
