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
		public void AstarTest()
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

			//act
			path = a.Process( grid, from, from );
			//assert
			Assert.IsTrue( path.First().Coordinates == from, "Same-Same: first coord" );
			Assert.IsTrue( path.Last().Coordinates == from, "Same-Same: last coord" );

			//arrange
			to = new Coordinates( gridWidth-1, gridHeight-1 );
			//act
			path = a.Process( grid, from, to );
			//assert
			Assert.IsTrue( path.Count() == 0, "Empty path" );
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
		[ExpectedException( typeof( System.IndexOutOfRangeException ) )]
        public void AstarOutOfRangeEndTest()
        {
            // arrange, act, assert
            new Astar().Process(GetTestGrid(), new Coordinates(1, 1), new Coordinates(-1, 1));
        }


	}
}
