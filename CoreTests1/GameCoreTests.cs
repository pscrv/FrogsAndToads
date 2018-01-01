using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameCore;
using Utilities;

namespace CoreTests
{



    [TestClass]
    public class GameCoreTests
    {
        private class Nim : Game<NimPosition>
        {
            public Nim()
                : base (new NimPlayer("Left"), new NimPlayer("Right"), new NimPosition(5))
            { }


            public override IEnumerable<NimPosition> GetLeftMoves(NimPosition position)
            {
                NimPosition nimPosition = position;
                if (nimPosition == null)
                    throw new InvalidOperationException("position is not a NimPosition.");

                List<NimPosition> options = new List<NimPosition>();
                for (int i = 0; i < nimPosition.Size - 1; i++)
                {
                    options.Add(new NimPosition(i));
                }

                return options;
            }
           
        }


        private class NimPosition : GamePosition
        {
            private int _size;

            public int Size => _size;

            public override GamePosition Reverse => this;

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


        private class NimPlayer : GamePlayer<NimPosition>
        {
            public NimPlayer(string label)
                : base (label)
            { }
            


            public override AttemptPlay<NimPosition> Play(IEnumerable<NimPosition> playOptions)
            {
                foreach (NimPosition options in playOptions)
                {
                    NimPosition nimPosition = options;
                    if (nimPosition == null)
                        return AttemptPlay<NimPosition>.Failure;

                    return AttemptPlay<NimPosition>.Success(nimPosition);
                }

                return AttemptPlay<NimPosition>.Failure;
            }
        }




        [TestMethod]
        public void PlayNim()
        {
            Game<NimPosition> nimGame = new Nim();
            nimGame.Play();

            Assert.AreEqual(2, nimGame.History.Count);
            Assert.AreEqual("Left", nimGame.Winner.Label);
        }
    }
}
