using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using PathfindingAlgorithms.Algorithms.Astar;
using PathfindingAlgorithms.Cells;
using UnitTests.Mocs;

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
                return $"{Coordinates}, {Weight}";
			}
		}


        int gridWidth = 5;
		int gridHeight = 5;

		ICell[,] GetUnpassableMiddleGrid()
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

        ICell[,] GetEmptyGrid()
        {
            var r = new ICell[gridWidth, gridHeight];
            for (int x = 0; x < gridWidth; ++x)
            {
                for (int y = 0; y < gridHeight; ++y)
                {
                    var c = new Cell(x, y);
                    r[x, y] = c;
                }
            }
            return r;
        }

        ICell[,] GetCirclesGrid()
        {
            var r = new ICell[gridWidth, gridHeight];
            for (int x = 0; x < gridWidth; ++x)
            {
                for (int y = 0; y < gridHeight; ++y)
                {
                    var c = new Cell(x, y);
                    r[x, y] = c;
                }
            }
            int midx = gridWidth / 2;
            int midy = gridHeight / 2;
            for (int i = midx - 1; i <= midx + 1; i++)
                for (int j = midy - 1; j <= midy + 1; j++)
                    r[i,j].Weight = -1;
            r[midx, midy].Weight = 0;
            r[1, 0].Weight = -1;
            r[0, 1].Weight = -1;
            r[1, 1].Weight = -1;
            return r;
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AstarCreating1()
        {
            var a = new Astar(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AstarCreating2()
        {
            var a = new Astar(new EmptyHeuristic(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AstarCreating3()
        {
            var a = new Astar(null, new EmptyAdjacement());
        }

        [TestMethod]
        public void AstarCreating4()
        {
            var a = new Astar(new EmptyHeuristic(), new EmptyAdjacement());
            Assert.IsFalse(a == null, "Object not created");
        }

        [TestMethod]
		public void AstarPathFoundTestEuc()
		{
			//arrange
			var a = new Astar(new EuclidianHeuristic(), new StraightAdjacement());
			var grid = GetUnpassableMiddleGrid();
			Coordinates from = new Coordinates( 0, 0 ), to = new Coordinates( 0, gridHeight-1 );
			//act
			var path = a.Process( grid, from, to );
			//assert
			Assert.IsTrue( path.First().Coordinates == from, "LT-LB: first coord" );
			Assert.IsTrue( path.Last().Coordinates == to, "LT-LB: last coord" );
		}

        [TestMethod]
        public void AstarPathFoundTestManh()
        {
            //arrange
            var a = new Astar(new ManhattanHeuristic(), new StraightAdjacement());
            var grid = GetUnpassableMiddleGrid();
            Coordinates from = new Coordinates(0, 0), to = new Coordinates(0, gridHeight - 1);
            //act
            var path = a.Process(grid, from, to);
            //assert
            Assert.IsTrue(path.First().Coordinates == from, "LT-LB: first coord");
            Assert.IsTrue(path.Last().Coordinates == to, "LT-LB: last coord");
        }

        [TestMethod]
        public void AstarSameCoordinatesTestEuc()
        {
            // arrange
            var a = new Astar(new EuclidianHeuristic(), new StraightAdjacement());
            var grid = GetUnpassableMiddleGrid();
            var coord = new Coordinates(0, 0);
            // act
            var path = a.Process(grid, coord, coord);
            // assert
            Assert.IsTrue(path.First().Coordinates == coord, "Same-Same: first coord");
            Assert.IsTrue(path.Last().Coordinates == coord, "Same-Same: last coord");
        }

        [TestMethod]
        public void AstarSameCoordinatesTestManh()
        {
            // arrange
            var a = new Astar(new ManhattanHeuristic(), new StraightAdjacement());
            var grid = GetUnpassableMiddleGrid();
            var coord = new Coordinates(0, 0);
            // act
            var path = a.Process(grid, coord, coord);
            // assert
            Assert.IsTrue(path.First().Coordinates == coord, "Same-Same: first coord");
            Assert.IsTrue(path.Last().Coordinates == coord, "Same-Same: last coord");
        }

        [TestMethod]
        public void AstarEmptyPathTestEuc()
        {
            // arrange
            var a = new Astar(new EuclidianHeuristic(), new StraightAdjacement());
            var grid = GetUnpassableMiddleGrid();
            Coordinates from = new Coordinates(0, 0), to = new Coordinates(gridWidth - 1, gridHeight - 1);
            // act
            var path = a.Process(grid, from, to); ;
            // assert
            Assert.IsTrue(path.Count() == 0, "Empty path");
        }

        [TestMethod]
        public void AstarEmptyPathTestManh()
        {
            // arrange
            var a = new Astar(new ManhattanHeuristic(), new StraightAdjacement());
            var grid = GetUnpassableMiddleGrid();
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
            new Astar(new EuclidianHeuristic(), new StraightAdjacement()).Process(null, new Coordinates(0, 0), new Coordinates(0, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(System.IndexOutOfRangeException))]
        public void AstarOutOfRangeStartTest()
        {
            // arrange, act, assert
            new Astar(new EuclidianHeuristic(), new StraightAdjacement()).Process(GetUnpassableMiddleGrid(), new Coordinates(gridWidth, gridHeight), new Coordinates(1, 1));
        }

        [TestMethod]
		[ExpectedException(typeof(System.IndexOutOfRangeException ))]
        public void AstarOutOfRangeEndTest()
        {
            // arrange, act, assert
            new Astar(new EuclidianHeuristic(), new StraightAdjacement()).Process(GetUnpassableMiddleGrid(), new Coordinates(1, 1), new Coordinates(-1, 1));
        }

        [TestMethod]
        public void AstarMoveDownTest1()
        {
            // arrange
            var a = new Astar(new EuclidianHeuristic(), new DownAdjacement());
            var grid = GetUnpassableMiddleGrid();
            Coordinates from = new Coordinates(0, 0), to = new Coordinates(1, gridHeight - 1);
            // act
            var path = a.Process(grid, from, to); ;
            // assert
            Assert.IsTrue(path.Count() == 0, "Empty path");
        }

        [TestMethod]
        public void AstarMoveDownTest2()
        {
            // arrange
            var a = new Astar(new EuclidianHeuristic(), new DownAdjacement());
            var grid = GetUnpassableMiddleGrid();
            Coordinates from = new Coordinates(0, 0), to = new Coordinates(0, gridHeight - 1);
            // act
            var path = a.Process(grid, from, to); ;
            // assert
            Assert.IsTrue(path.First().Coordinates == from, "first coord");
            Assert.IsTrue(path.Last().Coordinates == to, "last coord");
        }

        [TestMethod] 
        public void EuclidianDistTest()
        {
            var h = new EuclidianHeuristic();
            Coordinates a = new Coordinates(0, 3);
            Coordinates b = new Coordinates(4, 0);
            Coordinates c = new Coordinates(0, 0);

            double ab = h.Heuristic(a, b);
            double ac = h.Heuristic(a, c);
            double bc = h.Heuristic(b, c);

            Assert.AreEqual(5.0, ab, 1e-5, "wrong distance");
            Assert.AreEqual(3.0, ac, 1e-5, "wrong distance");
            Assert.AreEqual(4.0, bc, 1e-5, "wrong distance");
        }

        [TestMethod]
        public void ManhettanDistTest()
        {
            var h = new ManhattanHeuristic();
            Coordinates a = new Coordinates(0, 3);
            Coordinates b = new Coordinates(4, 0);
            Coordinates c = new Coordinates(0, 0);

            double ab = h.Heuristic(a, b);
            double ac = h.Heuristic(a, c);
            double bc = h.Heuristic(b, c);

            Assert.AreEqual(7.0, ab, 1e-5, "wrong distance");
            Assert.AreEqual(3.0, ac, 1e-5, "wrong distance");
            Assert.AreEqual(4.0, bc, 1e-5, "wrong distance");
        }

        [TestMethod]
        public void StraightAdjacementTest()
        {
            var a = new StraightAdjacement();
            var grid1 = GetEmptyGrid();
            var grid2 = GetCirclesGrid();
            Coordinates corner = new Coordinates(0, 0);
            Coordinates border = new Coordinates(0, grid1.GetLength(1) / 2);
            Coordinates middle1 = new Coordinates(grid1.GetLength(0) / 2, grid1.GetLength(1) / 2);
            Coordinates middle2 = new Coordinates(grid2.GetLength(0) / 2, grid2.GetLength(1) / 2);

            var ways1 = a.Adjacement(grid1, corner);
            var ways2 = a.Adjacement(grid1, border);
            var ways3 = a.Adjacement(grid1, middle1);
            var ways4 = a.Adjacement(grid2, corner);
            var ways5 = a.Adjacement(grid2, middle2);


            Assert.AreEqual(2, ways1.Count(), "empty corner");
            Assert.AreEqual(3, ways2.Count(), "empty border");
            Assert.AreEqual(4, ways3.Count(), "empty middle");
            Assert.AreEqual(0, ways4.Count(), "rounded corner");
            Assert.AreEqual(0, ways5.Count(), "rounded middle");

        }
    }
}
