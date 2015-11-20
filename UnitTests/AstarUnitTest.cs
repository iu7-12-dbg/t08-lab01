using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

		int gridWidth = 4;
		int gridHeight = 8;

		IList<IList<ICell>> GetTestGrid()
		{
			var r = new List<IList<ICell>>();
			for ( int x = 0; x < gridWidth; ++x )
			{
				r.Add( new List<ICell>() );
				for ( int y = 0; y < gridHeight; ++y )
				{
					var c = new Cell(x,y);
					r[x].Add( c );
				}
			}
			return r;
		}

		[TestMethod]
		public void AstarTest()
		{
			var grid = GetTestGrid();

			var a = new PathfindingAlgorithms.Algorithms.Astar();

			var path = (LinkedList<ICell>)a.Process( grid, new Coordinates( 0, 0 ), new Coordinates( gridWidth-1, gridHeight-1 ) );

			Assert.IsTrue( path.First.Value.Coordinates == grid[0][0].Coordinates, "LT-RB: first coord" );
			Assert.IsTrue( path.Last.Value.Coordinates == grid[gridWidth-1][gridHeight-1].Coordinates, "LT-RB: last coord" );


			path = (LinkedList<ICell>)a.Process( grid, new Coordinates( 1, 1 ), new Coordinates( 1, 1 ) );

			Assert.IsTrue( path.First.Value.Coordinates == grid[1][1].Coordinates, "Same-Same: first coord" );
			Assert.IsTrue( path.Last.Value.Coordinates == grid[1][1].Coordinates, "Same-Same: last coord" );
		}

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AstarNullCellMapTest()
        {
            // arrange, act, assert
            new Astar().Process(null, new Coordinates(0, 0), new Coordinates(0, 1));
        }
	}
}
