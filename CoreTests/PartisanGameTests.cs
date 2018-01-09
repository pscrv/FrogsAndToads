using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FrogsAndToadsCore;
using GameCore;

namespace CoreTests
{
    [TestClass]
    public class PartisanGameTests
    {
        FrogsAndToadsPlayChooser toadPlayer = new TrivialChooser("Toads");
        FrogsAndToadsPlayChooser frogPlayer = new TrivialChooser("Frogs");


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
    }
}
