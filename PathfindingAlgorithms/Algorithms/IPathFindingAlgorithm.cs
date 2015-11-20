using PathfindingAlgorithms.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms
{
    public interface IPathFindingAlgorithm
    {
        IEnumerable<ICell> Process(Coordinates[,] CellMap, Coordinates start, Coordinates end);
    }
}
