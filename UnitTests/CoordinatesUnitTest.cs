using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PathfindingAlgorithms.Cells;

namespace UnitTests
{
    [TestClass]
    public class CoordinatesUnitTest
    {
        [TestMethod]
        public void LessUnitTest()
        {
            // arrange

            var test = new Coordinates(5, 5);
            var more = new Coordinates(10, 10);
            var less = new Coordinates(0, 0);
            var notLess1 = new Coordinates(4, 6);
            var notLess2 = new Coordinates(6, 4);

            // act, assert
            Assert.IsTrue(less < test, "less");

            Assert.IsFalse(more < test, "more");
            Assert.IsFalse(notLess1 < test, "notLess1");
            Assert.IsFalse(notLess2 < test, "notLess2");
        }
    }
}
