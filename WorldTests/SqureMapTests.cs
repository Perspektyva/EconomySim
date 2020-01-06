using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using World.Map;

namespace WorldTests
{
    [TestClass]
    public class SqureMapTests
    {
        [TestMethod]
        public void Should_ConstructSingleCellMap()
        {
            SquareMap map = new SquareMap(1);

            map.GetCell(0, 0).Should().NotBeNull();
            map.GetCell(0, 0).Should().BeSameAs(map.GetCell(0, 0));
            map.GetCell(-1, -1).Should().BeNull();
        }

        [TestMethod]
        public void Should_ConstructMapOfRadius2()
        {
            SquareMap map = new SquareMap(2);

            map.GetCell(0, 0).Should().NotBeNull();
            map.GetCell(1, 1).Should().NotBeNull();
            map.GetCell(-1, -1).Should().NotBeNull();
            map.GetCell(-2, -2).Should().BeNull();
            map.GetCell(0, 0).Should().NotBeSameAs(map.GetCell(1, 1));
        }

        [TestMethod]
        public void Should_ConstructMapOfRadius10()
        {
            SquareMap map = new SquareMap(10);

            map.GetCell(0, 0).Should().NotBeNull();
            map.GetCell(9, 8).Should().NotBeNull();
            map.GetCell(-9, -7).Should().NotBeNull();
            map.GetCell(-10, -7).Should().BeNull();
        }
    }
}
