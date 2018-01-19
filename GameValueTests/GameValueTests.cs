using System;
using GameCore.GameValue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameValueTests
{
    [TestClass]
    public class GameValueTests
    {
        [TestMethod]
        public void GameValue()
        {
            GameValue gv;
            
            gv = new GameValue(0);
            gv = new GameValue(1);
            gv = new GameValue(2);
            gv = new GameValue(-1);
            gv = new GameValue(-2);
            
        }
    }
}
