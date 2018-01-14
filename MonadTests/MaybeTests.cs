using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Monads;

namespace MonadTests
{
    [TestClass]
    public class MaybeTests
    {
        Maybe<int> intMaybe;
        Maybe<string> stringMaybe;

        [TestMethod]
        public void Nothing()
        {
            intMaybe = Maybe<int>.Nothing();
            Assert.IsFalse(intMaybe.HasValue);
            Assert.AreEqual(default(int), intMaybe.Value);
            Assert.AreEqual("created from nothing", intMaybe.Explanation);
        }

        [TestMethod]
        public void SomeInt()
        {
            intMaybe = Maybe<int>.Some(7);
            Assert.IsTrue(intMaybe.HasValue);
            Assert.AreEqual(7, intMaybe.Value);
            Assert.AreEqual("ok", intMaybe.Explanation);

            intMaybe = Maybe<int>.Some(7, "why not");
            Assert.IsTrue(intMaybe.HasValue);
            Assert.AreEqual(7, intMaybe.Value);
            Assert.AreEqual("why not", intMaybe.Explanation);
        }


        [TestMethod]
        public void ToIntMaybe()
        {
            intMaybe = 7.ToMaybe();
            Assert.IsTrue(intMaybe.HasValue);
            Assert.AreEqual(7, intMaybe.Value);
            Assert.AreEqual("ok", intMaybe.Explanation);
        }

        [TestMethod]
        public void BindSome()
        {
            intMaybe = Maybe<int>.Some(7);
            
            stringMaybe = intMaybe.Bind(x => (x.ToString() + x.ToString()).ToMaybe());
            Assert.IsTrue(stringMaybe.HasValue);
            Assert.AreEqual("77", stringMaybe.Value);
            Assert.AreEqual("ok", stringMaybe.Explanation);

            stringMaybe = intMaybe.Bind<int, string>(null);
            Assert.IsFalse(stringMaybe.HasValue);
            Assert.AreEqual(default(string), stringMaybe.Value);
            Assert.AreEqual("lift was null", stringMaybe.Explanation);
        }

        [TestMethod]
        public void BindNothing()
        {
            intMaybe = Maybe<int>.Nothing();
            stringMaybe = intMaybe.Bind(x => (x.ToString() + x.ToString()).ToMaybe());
            Assert.IsFalse(stringMaybe.HasValue);
            Assert.AreEqual(default(string), stringMaybe.Value);


            stringMaybe = intMaybe.Bind<int, string>(null);
            Assert.IsFalse(stringMaybe.HasValue);
            Assert.AreEqual(default(string), stringMaybe.Value);
            Assert.AreEqual("lift was null", stringMaybe.Explanation);
        }

        [TestMethod]
        public void BindToNothing()
        {
            intMaybe = 7.ToMaybe();
            stringMaybe = intMaybe.Bind(x => Maybe<string>.Nothing());
            Assert.IsFalse(stringMaybe.HasValue);
            Assert.AreEqual(default(string), stringMaybe.Value);
        }

        [TestMethod]
        public void Select()
        {
            intMaybe = 7.ToMaybe();

            stringMaybe = intMaybe.Select<int, string>(null);
            Assert.IsFalse(stringMaybe.HasValue);
            Assert.AreEqual(default(string), stringMaybe.Value);
            Assert.AreEqual("select was null", stringMaybe.Explanation);


            stringMaybe = intMaybe.Select<int, string>(
                x => (3 * x).ToString());
            Assert.IsTrue(stringMaybe.HasValue);
            Assert.AreEqual("21", stringMaybe.Value);
            Assert.AreEqual("ok", stringMaybe.Explanation);

            stringMaybe = intMaybe.Select<int, string>(
                x => null);
            Assert.IsFalse(stringMaybe.HasValue);
            Assert.AreEqual(default(string), stringMaybe.Value);
            Assert.AreEqual("value was null", stringMaybe.Explanation);
        }

        [TestMethod]
        public void SelectMany()
        {
            intMaybe = 7.ToMaybe();

            stringMaybe = intMaybe.SelectMany<int, int, string>(
                x => (3 * x).ToMaybe(),
                null
                );
            Assert.IsFalse(stringMaybe.HasValue);
            Assert.AreEqual(default(string), stringMaybe.Value);
            Assert.AreEqual("select was null", stringMaybe.Explanation);

            stringMaybe = intMaybe.SelectMany<int, int, string>(
                null,
                null
                );
            Assert.IsFalse(stringMaybe.HasValue);
            Assert.AreEqual(default(string), stringMaybe.Value);
            Assert.AreEqual("select was null", stringMaybe.Explanation);

            stringMaybe = intMaybe.SelectMany<int, int, string>(
                null,
                (x, y) => "test"
                );
            Assert.IsFalse(stringMaybe.HasValue);
            Assert.AreEqual(default(string), stringMaybe.Value);
            Assert.AreEqual("lift was null", stringMaybe.Explanation);

            stringMaybe = intMaybe.SelectMany<int, int, string>(
                x => (2 * x).ToMaybe(),
                (x, y) => x.ToString() + y.ToString()
                );
            Assert.IsTrue(stringMaybe.HasValue);
            Assert.AreEqual("714", stringMaybe.Value);
            Assert.AreEqual("ok", stringMaybe.Explanation);

            stringMaybe = intMaybe.SelectMany<int, int, string>(
                x => Maybe<int>.Nothing(),
                (x, y) => x.ToString() + y.ToString()
                );
            Assert.IsFalse(stringMaybe.HasValue);
            Assert.AreEqual(default(string), stringMaybe.Value);
            Assert.AreEqual("created from nothing", stringMaybe.Explanation);
        }

        [TestMethod]
        public void LinqSyntax()
        {
            stringMaybe =
                from x in 9.ToMaybe()
                from y in "help".ToMaybe()
                from z in '#'.ToMaybe()
                select x + y + z;
            Assert.IsTrue(stringMaybe.HasValue);
            Assert.AreEqual("9help#", stringMaybe.Value);
            Assert.AreEqual("ok", stringMaybe.Explanation);

        }

        [TestMethod]
        public void MaybeNull()
        {
            string g = null;
            stringMaybe = g.ToMaybe();
            Assert.IsFalse(stringMaybe.HasValue);
            Assert.AreEqual(default(string), stringMaybe.Value);
            Assert.AreEqual("value was null", stringMaybe.Explanation);
        }
    }
}
 