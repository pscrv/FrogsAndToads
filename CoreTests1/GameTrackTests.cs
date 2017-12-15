using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameCore.Tests
{
    [TestClass()]
    public class GameTrackTests
    {
        [TestMethod()]
        public void GameTrack()
        {
            GameTrack track = new GameTrack(7);
            Assert.AreEqual(7, track.Length);
            for (int i = 0; i < track.Length; i++)
            {
                Assert.AreEqual(GamePiece.None, track[i], $"Failed for i = {i}.");
            }


            track = new GameTrack("T__F");
            Assert.AreEqual(4, track.Length);
            Assert.AreEqual("T", track[0].ToString());
            Assert.AreEqual("_", track[1].ToString());
            Assert.AreEqual("_", track[2].ToString());
            Assert.AreEqual("F", track[3].ToString());
        }

        [TestMethod()]
        public void AddPiece()
        {
            GameTrack track = new GameTrack(7);
            GamePiece piece = GamePiece.Dummy;
            track.AddPiece(piece, 0);

            Assert.AreEqual(0, track[piece]);
            Assert.AreEqual(GamePiece.Dummy, track[0]);
            Assert.AreNotEqual(GamePiece.Dummy, track[1]);
            Assert.AreEqual(GamePiece.None, track[1]);
        }

        [TestMethod()]
        public void RemovePiece()
        {
            GameTrack track = new GameTrack(7);
            GamePiece piece = GamePiece.Dummy;
            track.AddPiece(piece, 0);
            track.AddPiece(piece, 1);
            track.RemovePiece(piece, 0);

            Assert.AreNotEqual(piece, track[0]);
            Assert.AreEqual(piece, track[1]);
            Assert.AreEqual(1, track[piece]);
        }

        [TestMethod()]
        public void MovePiece()
        {
            GameTrack track = new GameTrack(7);
            GamePiece piece = GamePiece.Dummy;
            track.AddPiece(piece, 0);
            track.MovePiece(0, 6);

            for (int i = 0; i < track.Length - 1; i++)
            {
                Assert.AreEqual(GamePiece.None, track[i]);
            }
            Assert.AreEqual(piece, track[6]);
            Assert.AreEqual(6, track[piece]);
        }

        [TestMethod()]
        public void IndexRange()
        {
            GameTrack track = new GameTrack(7);
            GamePiece piece = GamePiece.Dummy;

            try
            {
                track.AddPiece(piece, 7);
                Assert.Fail("Failure to throw IndexOutOfRangeException (AddPiece).");
            }
            catch (IndexOutOfRangeException)
            { }
            
            try
            {
                track.RemovePiece(piece, -1);
                Assert.Fail("Failure to throw IndexOutOfRangeException (RemovePiece).");
            }
            catch (IndexOutOfRangeException)
            { }

            try
            {
                track.AddPiece(piece, 0);
                track.MovePiece(0, 7);
                Assert.Fail("Failure to throw IndexOutOfRangeException (MovePiece).");
            }
            catch (IndexOutOfRangeException)
            { }

        }


    }
}