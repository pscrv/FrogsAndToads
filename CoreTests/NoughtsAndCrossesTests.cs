using System;
using GameCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NoughtsAndCrossesCore;

namespace CoreTests
{
    [TestClass]
    public class NoughtsAndCrossesTests
    {
        [TestMethod]
        public void NoughtsAndCrossesPiece()
        {
            NoughtsAndCrossesPiece piece = new Cross();

            Assert.IsTrue(piece is Cross);
            Assert.IsFalse(piece is Space);
            Assert.IsFalse(piece is Nought);
            Assert.IsTrue(piece.Converse is Nought);
            Assert.IsTrue(piece.Converse.Converse is Cross);
            Assert.IsTrue(new Space().Converse is Space);
        }


        [TestMethod]
        public void NoughtsAndCrossesPosition()
        {
            int size = 3;
            NoughtsAndCrossesPosition position = new NoughtsAndCrossesPosition(size);

            Assert.AreEqual(size, position.Size);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Assert.IsTrue(position[i, j] is Space);
                }
            }
        }


        [TestMethod]
        public void NoughtsAndCrossesGame()
        {
            int size = 5;
            GamePlayer<NoughtsAndCrossesPosition> left = new DummyNoughtsAndCrossesPlayer();
            GamePlayer<NoughtsAndCrossesPosition> right = new DummyNoughtsAndCrossesPlayer();

            NoughtsAndCrossesGame game = new NoughtsAndCrossesGame(
                left, 
                right, 
                new NoughtsAndCrossesPosition(size));

            Assert.IsTrue(game.Position[0, 0] is Space);
            Assert.AreEqual(size, game.Position.Size);
        }
    }
}
