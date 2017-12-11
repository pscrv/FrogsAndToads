using System;
using System.Collections.Generic;
using GameCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities;

namespace CoreTests
{



    [TestClass]
    public class GameCoreTests
    {
        private class Nim : Game
        {
            public Nim()
                : base (new NimPlayer("Left"), new NimPlayer("Right"), new NimPosition(5))
            { }


            public override IEnumerable<GamePosition> GetLeftMoves(GamePosition position)
            {
                NimPosition nimPosition = position as NimPosition;
                if (nimPosition == null)
                    throw new InvalidOperationException("position is not a NimPosition.");

                List<NimPosition> options = new List<NimPosition>();
                for (int i = 0; i < nimPosition.Size - 1; i++)
                {
                    options.Add(new NimPosition(i));
                }

                return options;
            }

            public override IEnumerable<GamePosition> GetRightMoves(GamePosition position)
            {
                return GetLeftMoves(position);
            }
        }


        private class NimPosition : GamePosition
        {
            private int _size;

            public int Size => _size;

            public NimPosition(int size)
            {
                _size = size;
            }



            internal Try<GamePosition> Remove(int numberToRemove)
            {
                if (numberToRemove > _size)
                    return Try<GamePosition>.Failure;

                return Try<GamePosition>.Success(new NimPosition(_size - 1));
            }
        }


        private class NimPlayer : GamePlayer
        {
            public NimPlayer(string label)
                : base (label)
            { }
            


            internal override Try<GamePosition> Play(IEnumerable<GamePosition> playOptions)
            {
                foreach (GamePosition options in playOptions)
                {
                    NimPosition nimPosition = options as NimPosition;
                    if (nimPosition == null)
                        return Try<GamePosition>.Failure;

                    return Try<GamePosition>.Success(nimPosition);
                }

                return Try<GamePosition>.Failure;
            }
        }




        [TestMethod]
        public void PlayNim()
        {
            Game nimGame = new Nim();
            nimGame.Play();

            Assert.AreEqual(2, nimGame.History.Count);
            Assert.AreEqual("Left", nimGame.Winner.Label);
        }
    }
}
