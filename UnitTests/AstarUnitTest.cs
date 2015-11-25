using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using PathfindingAlgorithms.Algorithms;
using PathfindingAlgorithms.Cells;

namespace UnitTests
{
	[TestClass]
	public class AstarUnitTest
	{
		class Cell : ICell
		{
			public Cell(int x, int y)
			{
				Coordinates = new Coordinates( x, y );
			}

			public double Weight { get; set; }

			public Coordinates Coordinates { get; private set; }

			public override string ToString()
			{
				return String.Format( "[{0}, {1}]; {2}", Coordinates.X, Coordinates.Y, Weight );
			}
		}

		int gridWidth = 5;
		int gridHeight = 5;

		ICell[,] GetTestGrid()
		{
			var r = new ICell[gridWidth,gridHeight];
			for ( int x = 0; x < gridWidth; ++x )
			{
				for ( int y = 0; y < gridHeight; ++y )
				{
					var c = new Cell(x,y);
					r[x,y] = c;
				}
			}
			int mid = gridWidth / 2;
			for ( int y = 0; y < gridHeight; ++y )
			{
				r[mid, y].Weight = -1;
			}
			return r;
		}

		[TestMethod]
		public void AstarPathFoundTest()
		{
			//arrange
			var a = new PathfindingAlgorithms.Algorithms.Astar();
			var grid = GetTestGrid();
			Coordinates from = new Coordinates( 0, 0 ), to = new Coordinates( 0, gridHeight-1 );
			//act
			var path = a.Process( grid, from, to );
			//assert
			Assert.IsTrue( path.First().Coordinates == from, "LT-LB: first coord" );
			Assert.IsTrue( path.Last().Coordinates == to, "LT-LB: last coord" );
		}

        [TestMethod]
        public void AstarSameCoordinatesTest()
        {
            // arrange
            var a = new PathfindingAlgorithms.Algorithms.Astar();
            var grid = GetTestGrid();
            var coord = new Coordinates(0, 0);
            // act
            var path = a.Process(grid, coord, coord);
            // assert
            Assert.IsTrue(path.First().Coordinates == coord, "Same-Same: first coord");
            Assert.IsTrue(path.Last().Coordinates == coord, "Same-Same: last coord");
        }

        [TestMethod]
        public void AstarEmptyPathTest()
        {
            // arrange
            var a = new PathfindingAlgorithms.Algorithms.Astar();
            var grid = GetTestGrid();
            Coordinates from = new Coordinates(0, 0), to = new Coordinates(gridWidth - 1, gridHeight - 1);
            // act
            var path = a.Process(grid, from, to); ;
            // assert
            Assert.IsTrue(path.Count() == 0, "Empty path");
        }

        [TestMethod]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void AstarNullCellMapTest()
        {
            // arrange, act, assert
            new Astar().Process(null, new Coordinates(0, 0), new Coordinates(0, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(System.IndexOutOfRangeException))]
        public void AstarOutOfRangeStartTest()
        {
            // arrange, act, assert
            new Astar().Process(GetTestGrid(), new Coordinates(gridWidth, gridHeight), new Coordinates(1, 1));
        }

        [TestMethod]
		[ExpectedException(typeof(System.IndexOutOfRangeException ))]
        public void AstarOutOfRangeEndTest()
        {
            // arrange, act, assert
            new Astar().Process(GetTestGrid(), new Coordinates(1, 1), new Coordinates(-1, 1));
        }

        [TestMethod]
        public void ScoreTest()
        {
            // arrange
            var type = new PrivateType(typeof(Astar));
            var coord = new ValueCell(new Coordinates(), 0.3);
            // act
            double res = (double) type.InvokeStatic("Score", new object[] { coord });
            // assert
            Assert.IsTrue(Math.Abs(res - coord.Weight) < 1e-7, "Weight and score must be equal!");
        }

        [TestMethod]
        public void GetPassableNodeOutOfRangeTest()
        {
            // arrange
            var type = new PrivateType(typeof(Astar));
            var cell = new ValueCell[1, 1];
            // act, asser
            Assert.IsNull(type.InvokeStatic("GetPassableNode", new object[] { cell, -1, 0 }), "Left border check failed!");
            Assert.IsNull(type.InvokeStatic("GetPassableNode", new object[] { cell, 0, -1 }), "Bottom border check failed!");
            Assert.IsNull(type.InvokeStatic("GetPassableNode", new object[] { cell, 2, 0 }), "Right border check failed!");
            Assert.IsNull(type.InvokeStatic("GetPassableNode", new object[] { cell, 0, 2 }), "Up border check failed!");
        }

        [TestMethod]
        public void GetPassableNodeWeightTest()
        {
            // arrange
            var type = new PrivateType(typeof(Astar));
            var cell = new ValueCell[2, 2];
            cell[1, 1] = new ValueCell(new Coordinates(1, 1), -1);
            // act, assert
            Assert.IsNull(type.InvokeStatic("GetPassableNode", new object[] { cell, 1, 1 }));
        }

        [TestMethod]
        public void GetPassableNodeCorrectWeightTest()
        {
            // arrange
            var type = new PrivateType(typeof(Astar));
            var cell = new ValueCell[2, 2];
            cell[1, 1] = new ValueCell(new Coordinates(1, 1), 0.3);
            // act
            Coordinates res = (Coordinates) type.InvokeStatic("GetPassableNode", new object[] { cell, 1, 1 });
            // assert
            Assert.AreEqual(res.X, 1, "X coordinate must be the same!");
            Assert.AreEqual(res.Y, 1, "Y coordinate must be the same!");
        }
	}
}
