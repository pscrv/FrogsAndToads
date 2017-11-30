﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrogsAndToadsCore;

namespace CoreTests
{
    [TestClass]
    public class GameTests
    {       
        
        [TestMethod]
        public void Play()
        {
            Game game = new Game("T_F", new TrivialChooser(), new TrivialChooser());
            game.Play();

            Assert.AreEqual(4, game.History.Count);
            Assert.AreEqual("< T _ F >", game.History[0]);
            Assert.AreEqual("< _ T F >", game.History[1]);
            Assert.AreEqual("< F T _ >", game.History[2]);
            Assert.AreEqual("< F _ T >", game.History[3]);
        }

        [TestMethod]
        public void PlayOneMove()
        {
            Game game = new Game("T_F", new TrivialChooser(), new TrivialChooser());
            Assert.AreEqual("< T _ F >", game.CurrentPosition);

            game.PlayOneMove();
            Assert.AreEqual("< _ T F >", game.CurrentPosition);

            game.PlayOneMove();
            Assert.AreEqual("< F T _ >", game.CurrentPosition);

            game.PlayOneMove();
            Assert.AreEqual("< F _ T >", game.CurrentPosition);
        }


        [TestMethod]
        public void GameMoves()
        {
            Game game = new Game("T_F", new TrivialChooser(), new TrivialChooser());
            Assert.AreEqual("< T _ F >", game.CurrentPosition);

            foreach (GamePosition position in game.GameMoves())
            { }

            Assert.AreEqual("< F _ T >", game.CurrentPosition);
        }
    }
}