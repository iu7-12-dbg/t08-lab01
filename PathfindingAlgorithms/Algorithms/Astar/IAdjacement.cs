using PathfindingAlgorithms.Cells;
using System.Collections.Generic;

namespace PathfindingAlgorithms.Algorithms.Astar
{
    public interface IAdjacement
    {
        IEnumerable<Coordinates> Adjacement(ICell[,] cell, Coordinates to);
    }
}
