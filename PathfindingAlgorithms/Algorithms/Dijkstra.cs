using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathfindingAlgorithms.Cells;

namespace PathfindingAlgorithms.Algorithms
{
    class Dijkstra : IPathFindingAlgorithm
    {
        public IEnumerable<ICell> Process(IList<IList<ICell>> CellMap, Tuple<int, int> start, Tuple<int, int> end)
        {
            if (start.Item1 >= CellMap.Count || start.Item2 >= CellMap[start.Item1].Count)
                throw new ArgumentOutOfRangeException("start");
            if (end.Item1 >= CellMap.Count || end.Item2 >= CellMap[start.Item1].Count)
                throw new ArgumentOutOfRangeException("start");

            throw new NotImplementedException();
        }
    }
}
