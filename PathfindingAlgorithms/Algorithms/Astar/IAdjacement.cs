using PathfindingAlgorithms.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms.Astar
{
    public interface IAdjacement
    {
        IEnumerable<Coordinates> Adjacement(ICell[,] cell, Coordinates to);
    }
}
