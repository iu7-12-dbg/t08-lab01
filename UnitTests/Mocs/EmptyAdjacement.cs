using PathfindingAlgorithms.Algorithms.Astar;
using PathfindingAlgorithms.Cells;
using System.Collections.Generic;

namespace UnitTests.Mocs
{
    class EmptyAdjacement : IAdjacement
    {
        public IEnumerable<Coordinates> Adjacement(ICell[,] cell, Coordinates to)
        {
            return null;
        }
    }
}
