using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Model.Tests
{
    [TestClass()]
    public class StackTests
    {
        [TestMethod()]
        public void MergeStacks_Tests()
        {
            Good iron = new Good("Iron");
            Stack expected;
            Stack actual;

            expected = null;
            actual = Stack.MergeStacks(null, null);
            Assert.AreEqual(expected, actual);

            expected = new Stack(iron, 5.0);
            actual = Stack.MergeStacks(new Stack(iron, 5.0), null);
            Assert.AreEqual(expected, actual);

            expected = new Stack(iron, 5.0);
            actual = Stack.MergeStacks(null, new Stack(iron, 5.0));
            Assert.AreEqual(expected, actual);

            expected = new Stack(iron, 12.4);
            actual = Stack.MergeStacks(new Stack(iron, 7.4), new Stack(iron, 5.0));
            Assert.AreEqual(expected, actual);

            expected = new Stack(iron, 5.0);
            actual = Stack.MergeStacks(new Stack(iron, 0.0), new Stack(iron, 5.0));
            Assert.AreEqual(expected, actual);
        }
        [ExpectedException(
            typeof(ArgumentException))]
        [TestMethod()]
        public void MergeStacks_ThrowsIfDifferentType()
        {
            Good food = new Good("Food");
            Good iron = new Good("Iron");

            Stack.MergeStacks(new Stack(food, 1.0), new Stack(iron, 4.0));

            
        }

        [TestMethod()]
        public void SplitStack_Tests()
        {
            Good food = new Good("food");
            Stack stack;
            Tuple<Stack, Stack> actual;
            Tuple<Stack, Stack> expected;

            stack = new Stack(food, 40.0);
            expected = new Tuple<Stack, Stack>(new Stack(food, 15.0), new Stack(food, 25.0));
            actual = Stack.SplitStack(stack, 15.0);
            Assert.AreEqual(expected, actual);

            stack = new Stack(food, 40.0);
            expected = new Tuple<Stack, Stack>(new Stack(food, 40.0), new Stack(food, 0.0));
            actual = Stack.SplitStack(stack, 40.0);
            Assert.AreEqual(expected, actual);

            stack = new Stack(food, 87.9);
            expected = new Tuple<Stack, Stack>(new Stack(food, 0.0), new Stack(food, 87.9));
            actual = Stack.SplitStack(stack, 0.0);
            Assert.AreEqual(expected, actual);

            expected = new Tuple<Stack, Stack>(null, null);
            actual = Stack.SplitStack(null, 0.0);
            Assert.AreEqual(expected, actual);
        }

        [ExpectedException(
            typeof(ArgumentException))]
        [TestMethod()]
        public void SplitStack_ThrowsWhenNegativeArgument()
        {
            Stack.SplitStack(new Stack(new Good("Das"), 7.0), -18.98);
        }

        [ExpectedException(
            typeof(ArgumentException))]
        [TestMethod()]
        public void SplitStack_ThrowsWhenRequestExceeds()
        {
            Stack.SplitStack(new Stack(new Good("Das"), 56.5), 78.9);
        }

        [ExpectedException(
            typeof(ArgumentException))]
        [TestMethod()]
        public void Stack_GivenNegativeQuantityToConstructor_Throws()
        {
            new Stack(new Good("fro"), -9.8);
        }

        [ExpectedException(
            typeof(ArgumentNullException))]
        [TestMethod()]
        public void Stack_GivenNoGoodToConstructor_Throws()
        {
            new Stack(null, 9.9);
        }
    }
}
