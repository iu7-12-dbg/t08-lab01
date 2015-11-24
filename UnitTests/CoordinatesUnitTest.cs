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

            var test1 = new Coordinates(5, 5);
			var test2 = new Coordinates( 5, 5 );
            var more = new Coordinates(10, 10);
            var less = new Coordinates(0, 0);
            var notLess1 = new Coordinates(4, 6);
            var notLess2 = new Coordinates(6, 4);

            // act, assert
            Assert.IsTrue(less < test1, "less");
            Assert.IsTrue(more > test1, "more");
			Assert.IsTrue( test1 == test2, "equal" );
			Assert.IsFalse( test1 != test2, "not equal" );
            Assert.IsFalse(notLess1 < test1, "notLess1");
            Assert.IsFalse(notLess2 < test1, "notLess2");
        }
    }
}
