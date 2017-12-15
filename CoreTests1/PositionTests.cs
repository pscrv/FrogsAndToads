using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FrogsAndToadsCore;
using System.Collections.Generic;

namespace CoreTests
{
    [TestClass]
    public class PositionTests
    {
        [TestMethod]
        public void Construction()
        {
            FrogsAndToadsPosition position1 = new FrogsAndToadsPosition(1, 1, 1);
            FrogsAndToadsPosition position2 = new FrogsAndToadsPosition(2, 1, 1);
            FrogsAndToadsPosition position3 = new FrogsAndToadsPosition(3, 1, 2);
            FrogsAndToadsPosition position4 = new FrogsAndToadsPosition(4, 2, 3);
            FrogsAndToadsPosition position5 = new FrogsAndToadsPosition("TF__FTT");

            Assert.AreEqual("< T _ F >", position1.ToString());
            Assert.AreEqual("< T T _ F >", position2.ToString());
            Assert.AreEqual("< T T T _ F F >", position3.ToString());
            Assert.AreEqual("< T T T T _ _ F F F >", position4.ToString());
            Assert.AreEqual("< T F _ _ F T T >", position5.ToString());

            try
            {
                string value = "TF_x_FTT";
                FrogsAndToadsPosition bad = new FrogsAndToadsPosition(value);
                Assert.Fail($"Failed to throw an ArgumentException with positionString = {value}");
            }
            catch (ArgumentException)
            { }

        }


        [TestMethod]
        public void StaticConstructor()
        {
            // Will fail, when MakeInitialPosition is properly implemented

            FrogsAndToadsPosition position = FrogsAndToadsPosition.MakeInitialPosition();
            Assert.AreEqual("< T _ F >", position.ToString());
        }


        [TestMethod]
        public void CanMovePiece()
        {
            FrogsAndToadsPosition position = new FrogsAndToadsPosition(2, 2, 2);

            try
            {
                int value = -1;
                position.CanMovePiece(value);
                Assert.Fail($"Failed to throw an IndexOutOfRangeException for index {value}.");
            }
            catch (IndexOutOfRangeException)
            { }

            try
            {
                int value = 6;
                position.CanMovePiece(value);
                Assert.Fail($"Failed to throw an IndexOutOfRangeException for index {value}.");
            }
            catch (IndexOutOfRangeException)
            { }

            Assert.IsFalse(position.CanMovePiece(0));
            Assert.IsTrue(position.CanMovePiece(1));
            Assert.IsFalse(position.CanMovePiece(2));
            Assert.IsFalse(position.CanMovePiece(3));
            Assert.IsTrue(position.CanMovePiece(4));
            Assert.IsFalse(position.CanMovePiece(5));
        }


        [TestMethod]
        public void MovePiece()
        {
            FrogsAndToadsPosition position = new FrogsAndToadsPosition(2, 2, 2);

            try
            {
                int value = -1;
                position =  position.MovePiece(value);
                Assert.Fail($"Failed to throw an IndexOutOfRangeException for index {value}.");
            }
            catch (IndexOutOfRangeException)
            { }

            try
            {
                int value = 6;
                position = position.MovePiece(value);
                Assert.Fail($"Failed to throw an IndexOutOfRangeException for index {value}.");
            }
            catch (IndexOutOfRangeException)
            { }


            position = position.MovePiece(1);
            Assert.AreEqual("< T _ T _ F F >", position.ToString());

            position = position.MovePiece(4);
            Assert.AreEqual("< T _ T F _ F >", position.ToString());

            position = position.MovePiece(2);
            Assert.AreEqual("< T _ _ F T F >", position.ToString());
        }

        [TestMethod]
        public void GetAllPossibleMoves()
        {
            FrogsAndToadsPosition position = new FrogsAndToadsPosition(2, 2, 2);

            List<int> possibleMoves = position.GetAllPossibleMoves();
            Assert.AreEqual(2, possibleMoves.Count);
            Assert.IsTrue(possibleMoves.Contains(1));
            Assert.IsTrue(possibleMoves.Contains(4));

            position = position.MovePiece(1);
            position = position.MovePiece(4);
            possibleMoves = position.GetAllPossibleMoves();
            Assert.AreEqual(4, possibleMoves.Count);
            Assert.IsTrue(possibleMoves.Contains(0));
            Assert.IsTrue(possibleMoves.Contains(2));
            Assert.IsTrue(possibleMoves.Contains(3));
            Assert.IsTrue(possibleMoves.Contains(5));
        }


        [TestMethod]
        public void GetPossibleToadMoves()
        {
            FrogsAndToadsPosition position = new FrogsAndToadsPosition(2, 2, 2);

            List<int> possibleMoves = position.GetPossibleToadMoves();
            Assert.AreEqual(1, possibleMoves.Count);
            Assert.IsTrue(possibleMoves.Contains(1));

            position = position.MovePiece(1);
            position = position.MovePiece(4);
            possibleMoves = position.GetPossibleToadMoves();
            Assert.AreEqual(2, possibleMoves.Count);
            Assert.IsTrue(possibleMoves.Contains(0));
            Assert.IsTrue(possibleMoves.Contains(2));
        }


        [TestMethod]
        public void GetPossibleFrogMoves()
        {
            FrogsAndToadsPosition position = new FrogsAndToadsPosition(2, 2, 2);

            List<int> possibleMoves = position.GetPossibleFrogMoves();
            Assert.AreEqual(1, possibleMoves.Count);
            Assert.IsTrue(possibleMoves.Contains(4));

            position = position.MovePiece(1);
            position = position.MovePiece(4);
            possibleMoves = position.GetPossibleFrogMoves();
            Assert.AreEqual(2, possibleMoves.Count);
            Assert.IsTrue(possibleMoves.Contains(3));
            Assert.IsTrue(possibleMoves.Contains(5));
        }
    }
}
