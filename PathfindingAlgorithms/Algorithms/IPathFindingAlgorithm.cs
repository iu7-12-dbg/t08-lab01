using PathfindingAlgorithms.Cells;
using System.Collections.Generic;

namespace PathfindingAlgorithms.Algorithms
{
    public interface IPathFindingAlgorithm
    {
        IEnumerable<ICell> Process(ICell[,] CellMap, Coordinates start, Coordinates end);
    }
}
