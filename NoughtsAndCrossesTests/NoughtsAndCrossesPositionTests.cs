using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using GameCore;
using NoughtsAndCrossesCore;

namespace NoughtsAndCrossesTests
{
    [TestClass]
    public class NoughtsAndCrossesPositionTests
    {
        NoughtsAndCrossesPosition onePosition = new NoughtsAndCrossesPosition(1);
        NoughtsAndCrossesPosition twoPosition = new NoughtsAndCrossesPosition(2);
        NoughtsAndCrossesPosition threePosition = new NoughtsAndCrossesPosition(3);

        NoughtsAndCrossesPiece testPiece;
        NoughtsAndCrossesPosition result;
        IEnumerable<GamePosition> options;


        [TestMethod]
        public void Indexing()
        {
            testPiece = onePosition[0, 0];
            Assert.AreEqual(Space.Instance, testPiece);

            testPiece = twoPosition[0, 1];
            Assert.AreEqual(Space.Instance, testPiece);

            testPiece = twoPosition.Play(1, 0, Cross.Instance)[1, 0];
            Assert.AreEqual(Cross.Instance, testPiece);
        }


        [TestMethod]
        public void IndexingExceptions()
        {
            try
            {
                testPiece = onePosition[2, 0];
                Assert.Fail("Failure to throw IndexOutOfRangeException.");
            }
            catch (IndexOutOfRangeException)
            { }


            try
            {
                testPiece = twoPosition[0, -1];
                Assert.Fail("Failure to throw IndexOutOfRangeException.");
            }
            catch (IndexOutOfRangeException)
            { }
        }


        [TestMethod]
        public void Play()
        {
            result = onePosition.Play(0, 0, Cross.Instance);
            Assert.AreEqual(Cross.Instance, result[0, 0]);

            result = twoPosition.Play(1, 0, Nought.Instance);
            Assert.AreEqual(Nought.Instance, result[1, 0]);
            Assert.AreEqual(Space.Instance, result[0, 0]);
            Assert.AreEqual(Space.Instance, result[0, 1]);
            Assert.AreEqual(Space.Instance, result[1, 1]);
        }


        [TestMethod]
        public void PlayExceptions()
        {
            try
            {
                result = onePosition.Play(2, 0, Cross.Instance);
                Assert.Fail("Failure to throw IndexOutOfRangeException.");
            }
            catch (IndexOutOfRangeException)
            { }


            try
            {
                result = twoPosition.Play(0, -1, Cross.Instance);
                Assert.Fail("Failure to throw IndexOutOfRangeException.");
            }
            catch (IndexOutOfRangeException)
            { }


            try
            {
                result = twoPosition.Play(0, 0, Space.Instance);
                Assert.Fail("Failure to throw InvalidOperationException.");
            }
            catch (InvalidOperationException)
            { }


            try
            {
                result = twoPosition
                    .Play(0, 0, Cross.Instance)
                    .Play(0, 0, Nought.Instance);
                Assert.Fail("Failure to throw InvalidOperationException.");
            }
            catch (InvalidOperationException)
            { }


            try
            {
                result = threePosition
                    .Play(1, 1, Space.Instance);
                Assert.Fail("Failure to throw InvalidOperationException.");
            }
            catch (InvalidOperationException)
            { }


            try
            {
                result = threePosition
                    .Play(1, 1, null);
                Assert.Fail("Failure to throw ArgumentException.");
            }
            catch (ArgumentException)
            { }
        }


        [TestMethod]
        public void GetOptions()
        {
            options = onePosition.GetLeftOptions();
            Assert.AreEqual(1, options.Count());

            options = onePosition.GetRightOptions();
            Assert.AreEqual(1, options.Count());

            options = onePosition
                .Play(0, 0, Cross.Instance)
                .GetLeftOptions();
            Assert.AreEqual(0, options.Count());

            options = onePosition
                .Play(0, 0, Cross.Instance)
                .GetRightOptions();
            Assert.AreEqual(0, options.Count());
            

            options = twoPosition
                .GetLeftOptions();
            Assert.AreEqual(4, options.Count());
            
            options = twoPosition
                .Play(0, 0, Cross.Instance)
                .Play(1, 1, Nought.Instance)
                .GetLeftOptions();
            Assert.AreEqual(2, options.Count());
        }


        [TestMethod]
        public void IsEndPosition()
        {
            Assert.IsFalse(onePosition.IsEndPosition);
            Assert.IsFalse(twoPosition.IsEndPosition);
            Assert.IsFalse(threePosition.IsEndPosition);
            
            result = onePosition
                .Play(0, 0, Cross.Instance);
            Assert.IsTrue(result.IsEndPosition);

            result = twoPosition
                .Play(0, 0, Cross.Instance)
                .Play(1, 0, Cross.Instance);
            Assert.IsTrue(result.IsEndPosition);

            NoughtsAndCrossesPosition nearly = threePosition
                .Play(0, 0, Cross.Instance)
                .Play(1, 0, Cross.Instance)
                .Play(0, 1, Cross.Instance)
                .Play(1, 1, Cross.Instance);
            Assert.IsFalse(nearly.IsEndPosition);

            result = nearly
                .Play(2, 2, Cross.Instance);
            Assert.IsTrue(result.IsEndPosition);

            result = nearly
                .Play(1, 2, Cross.Instance);
            Assert.IsTrue(result.IsEndPosition);

            result = nearly
                .Play(2, 1, Cross.Instance);
            Assert.IsTrue(result.IsEndPosition);

            result = nearly
                .Play(0, 2, Cross.Instance);
            Assert.IsTrue(result.IsEndPosition);

            result = nearly
                .Play(2, 0, Cross.Instance);
            Assert.IsTrue(result.IsEndPosition);

        }
    }
}
