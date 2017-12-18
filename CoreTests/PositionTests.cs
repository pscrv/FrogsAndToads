using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

using FrogsAndToadsCore;

namespace CoreTests
{
    [TestClass]
    public class PositionTests
    {
        FrogsAndToadsPosition position;
        List<FrogsAndToadsMove> possibleMoves;
        List<int> _sources => possibleMoves.Select(x => x.Source).ToList();
        List<int> _targets => possibleMoves.Select(x => x.Target).ToList();
        FrogsAndToadsMove _move;



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

            position = FrogsAndToadsPosition.MakeInitialPosition();
            Assert.AreEqual("< T _ F >", position.ToString());
        }


        [TestMethod]
        public void CanMovePiece()
        {
            position = new FrogsAndToadsPosition(2, 2, 2);

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
        public void PlayMove()
        {
            position = new FrogsAndToadsPosition(2, 2, 2);

            _move = new FrogsAndToadsMove(1, 2);
            position = position.PlayMove(_move);
            Assert.AreEqual("< T _ T _ F F >", position.ToString());

            _move = new FrogsAndToadsMove(4, 3);
            position = position.PlayMove(_move);
            Assert.AreEqual("< T _ T F _ F >", position.ToString());

            _move = new FrogsAndToadsMove(2, 4);
            position = position.PlayMove(_move);
            Assert.AreEqual("< T _ _ F T F >", position.ToString());
        }

                

        [TestMethod]
        public void GetAllPossibleMoves()
        {
            position = new FrogsAndToadsPosition(2, 2, 2);
            
            possibleMoves = position.GetAllPossibleMoves();
            Assert.AreEqual(2, possibleMoves.Count);
            Assert.IsTrue(_sources.Contains(1));
            Assert.IsTrue(_sources.Contains(4));
            Assert.IsTrue(_targets.Contains(2));
            Assert.IsTrue(_targets.Contains(3));

            _move = new FrogsAndToadsMove(1, 2);
            position = position.PlayMove(_move);

            _move = new FrogsAndToadsMove(4, 3);
            position = position.PlayMove(_move);

            possibleMoves = position.GetAllPossibleMoves();
            Assert.AreEqual(4, possibleMoves.Count);
            Assert.IsTrue(_sources.Contains(0));
            Assert.IsTrue(_sources.Contains(2));
            Assert.IsTrue(_sources.Contains(3));
            Assert.IsTrue(_sources.Contains(5));
            Assert.IsTrue(_targets.Contains(1));
            Assert.IsTrue(_targets.Contains(4));
        }

        [TestMethod]
        public void GetPossibleToadMoves()
        {
            position = new FrogsAndToadsPosition(2, 2, 2);
            possibleMoves = position.GetPossibleToadMoves();
            Assert.AreEqual(1, possibleMoves.Count);
            Assert.IsTrue(_sources.Contains(1));
            Assert.IsTrue(_targets.Contains(2));

            _move = new FrogsAndToadsMove(1, 2);
            position = position.PlayMove(_move);

            _move = new FrogsAndToadsMove(4, 3);
            position = position.PlayMove(_move);

            possibleMoves = position.GetPossibleToadMoves();
            Assert.AreEqual(2, possibleMoves.Count);
            Assert.IsTrue(_sources.Contains(0));
            Assert.IsTrue(_sources.Contains(2));
            Assert.IsTrue(_targets.Contains(1));
            Assert.IsTrue(_targets.Contains(4));
        }


        [TestMethod]
        public void GetPossibleFrogMoves()
        {
            FrogsAndToadsPosition position = new FrogsAndToadsPosition(2, 2, 2);

            possibleMoves = position.GetPossibleFrogMoves();
            Assert.AreEqual(1, possibleMoves.Count);
            Assert.IsTrue(_sources.Contains(4));
            Assert.IsTrue(_targets.Contains(3));

            _move = new FrogsAndToadsMove(1, 2);
            position = position.PlayMove(_move);

            _move = new FrogsAndToadsMove(4, 3);
            position = position.PlayMove(_move);

            possibleMoves = position.GetPossibleFrogMoves();
            Assert.AreEqual(2, possibleMoves.Count);
            Assert.IsTrue(_sources.Contains(3));
            Assert.IsTrue(_sources.Contains(5));
            Assert.IsTrue(_targets.Contains(1));
            Assert.IsTrue(_targets.Contains(4));        }
    }
}
