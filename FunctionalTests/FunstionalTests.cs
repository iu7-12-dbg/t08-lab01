using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using PathfindingAlgorithms.Algorithms;
using PathfindingAlgorithms.Cells;

namespace FunctionalTests
{
    [TestClass]
    public class FunctionalTest
    {
        class Cell : ICell
        {
            public Cell(int x, int y)
            {
                Coordinates = new Coordinates(x, y);
            }

            public double Weight { get; set; }

            public Coordinates Coordinates { get; private set; }

            public override string ToString()
            {
                return String.Format("[{0}, {1}]; {2}", Coordinates.X, Coordinates.Y, Weight);
            }
        }

        int gridWidth = 5;
        int gridHeight = 5;

        ICell[,] GetTestGrid()
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
            int mid = gridWidth / 2;
            for (int y = 0; y < gridHeight; ++y)
            {
                r[mid, y].Weight = -1;
            }
            return r;
        }

        ICell[,] GetRandomGrid(int seed)
        {
            Random rand = new Random(seed);
            var r = new ICell[gridWidth, gridHeight];
            for (int x = 0; x < gridWidth; ++x)
            {
                for (int y = 0; y < gridHeight; ++y)
                {
                    int ra = rand.Next();
                    double w;
                    if ((ra % 100) < 80)
                        w = (ra % 6) / 5.0;
                    else
                        w = -1.0;
                    var c = new Cell(x, y);
                    c.Weight = w;
                    r[x, y] = c;
                }
            }
            int mid = gridWidth / 2;
            for (int y = 0; y < gridHeight; ++y)
            {
                r[mid, y].Weight = -1;
            }
            return r;
        }

        double PathLen(IEnumerable<ICell> path)
        {
            if (path.Count() == 0)
            {
                return double.PositiveInfinity;
            }
            else
            {
                return path.Aggregate(0.0, (acc, cell) => acc + cell.Weight);
            }
        }

        [TestMethod]
        public void PathEndpointsTest()
        {
            //arrange
            var a = new Astar();
            var d = new Dijkstra();
            var grid = GetTestGrid();
            Coordinates from = new Coordinates(0, 0), to = new Coordinates(0, gridHeight - 1);
            //act
            var path1 = a.Process(grid, from, to);
            var path2 = d.Process(grid, from, to);
            //compare
            Assert.IsTrue(path1.First().Coordinates == path2.First().Coordinates, "first coord");
            Assert.IsTrue(path1.Last().Coordinates == path2.Last().Coordinates, "last coord");
        }
        [TestMethod]
        public void PathLenTest1()
        {
            //arrange
            var a = new Astar();
            var d = new Dijkstra();
            var grid = GetTestGrid();
            Coordinates from = new Coordinates(0, 0), to = new Coordinates(0, gridHeight - 1);
            //act
            var path1 = a.Process(grid, from, to);
            var path2 = d.Process(grid, from, to);
            //compare
            Assert.IsTrue(PathLen(path1) == PathLen(path2), "path length");
        }

        [TestMethod]
        public void PathLenTest2()
        {

            var a = new Astar();
            var d = new Dijkstra();
            double l1, l2;
            ICell[,] grid;
            IEnumerable<ICell> path1, path2;
            Coordinates from = new Coordinates(0, 0), to = new Coordinates(gridWidth - 1, gridHeight - 1);


            grid = GetRandomGrid(0);
            path1 = a.Process(grid, from, to);
            path2 = d.Process(grid, from, to);
            l1 = PathLen(path1);
            l2 = PathLen(path2);
            Assert.IsTrue(l1 == l2, "path length. A* = " + l1.ToString() + " Dijkstra = " + l2.ToString());
            grid = GetRandomGrid(15);
            path1 = a.Process(grid, from, to);
            path2 = d.Process(grid, from, to);
            l1 = PathLen(path1);
            l2 = PathLen(path2);
            Assert.IsTrue(l1 == l2, "path length. A* = " + l1.ToString() + " Dijkstra = " + l2.ToString());
            grid = GetRandomGrid(22);
            path1 = a.Process(grid, from, to);
            path2 = d.Process(grid, from, to);
            l1 = PathLen(path1);
            l2 = PathLen(path2);
            Assert.IsTrue(l1 == l2, "path length. A* = " + l1.ToString() + " Dijkstra = " + l2.ToString());
            grid = GetRandomGrid(0xEDA);
            path1 = a.Process(grid, from, to);
            path2 = d.Process(grid, from, to);
            l1 = PathLen(path1);
            l2 = PathLen(path2);
            Assert.IsTrue(l1 == l2, "path length. A* = " + l1.ToString() + " Dijkstra = " + l2.ToString());
        }
    }
}