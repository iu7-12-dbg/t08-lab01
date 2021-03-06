// <copyright file="AstarTest.cs" company="Microsoft">Copyright © Microsoft 2015</copyright>
using System;
using System.Collections.Generic;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PathfindingAlgorithms.Algorithms.Astar;
using PathfindingAlgorithms.Cells;

namespace PathfindingAlgorithms.Algorithms.Astar.Tests
{
    /// <summary>Этот класс содержит параметризованные модульные тесты для Astar</summary>
    [PexClass(typeof(global::PathfindingAlgorithms.Algorithms.Astar.Astar))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class AstarTest
    {
        /// <summary>Тестовая заглушка для .ctor(IHeuristic, IAdjacement)</summary>
        [PexMethod]
        public global::PathfindingAlgorithms.Algorithms.Astar.Astar ConstructorTest(IHeuristic heuristic, IAdjacement adjacement)
        {
            global::PathfindingAlgorithms.Algorithms.Astar.Astar target
               = new global::PathfindingAlgorithms.Algorithms.Astar.Astar
                     (heuristic, adjacement);
            return target;
            // TODO: добавление проверочных утверждений в метод AstarTest.ConstructorTest(IHeuristic, IAdjacement)
        }

        /// <summary>Тестовая заглушка для Process(ICell[,], Coordinates, Coordinates)</summary>
        [PexMethod(MaxConstraintSolverTime = 8, MaxRunsWithoutNewTests = 200, MaxConditions = 1000)]
        public IEnumerable<ICell> ProcessTest(
            [PexAssumeUnderTest]global::PathfindingAlgorithms.Algorithms.Astar.Astar target,
            ICell[,] cells,
            Coordinates from,
            Coordinates to
        )
        {
            PexAssume.IsNotNull(cells);
            PexAssume.IsTrue(cells.GetLength(0)* cells.GetLength(1) > 0);
            PexAssume.IsTrue(from.Inside(new Coordinates(cells.GetLength(0) - 1, cells.GetLength(1) - 1)));
            PexAssume.IsTrue(to.Inside(new Coordinates(cells.GetLength(0) - 1, cells.GetLength(1) - 1)));
            PexAssume.IsTrue(cells.GetLowerBound(0) == 0);
            PexAssume.IsTrue(cells.GetLowerBound(1) == 0);
            bool f = true;
            for (int x = cells.GetLowerBound(0); x <= cells.GetUpperBound(0); x++)
            {
                for (int y = cells.GetLowerBound(1); y <= cells.GetUpperBound(1); y++)
                {
                    PexAssume.IsNotNull(cells[x, y]);
                    PexAssume.IsNotNull(cells[x, y].Coordinates);
                    f &= cells[x, y].Coordinates.Equals(new Coordinates(x, y));
                }
            }
            PexAssume.IsTrue(f);
            IEnumerable<ICell> result = target.Process(cells, from, to);
            return result;
            // TODO: добавление проверочных утверждений в метод AstarTest.ProcessTest(Astar, ICell[,], Coordinates, Coordinates)
        }
    }
}
