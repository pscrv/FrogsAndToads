using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GameCore;
using Monads;
using System.Linq;

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


            public override IEnumerable<NimPosition> GetLeftOptions(NimPosition position)
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

            public override IEnumerable<NimPosition> GetRightOptions(NimPosition position)
            {
                return GetLeftOptions(position);
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



            internal Maybe<GamePosition> Remove(int numberToRemove)
            {
                if (numberToRemove > _size)
                    return Maybe<GamePosition>.Nothing();

                return Maybe<GamePosition>.Some(new NimPosition(_size - 1));
            }
        }


        private class NimPlayer : GamePlayer<NimPosition>
        {
            public NimPlayer(string label)
                : base (label)
            { }
            


            public override Maybe<NimPosition> Play(IEnumerable<NimPosition> playOptions)
            {
                if (playOptions.Count() == 0)
                    return Maybe<NimPosition>.Nothing();
                
                return playOptions.First().ToMaybe();
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
