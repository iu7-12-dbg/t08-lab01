using PathfindingAlgorithms.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms.Astar
{
    public interface IHeuristic
    {
        double Heuristic(Coordinates a, Coordinates b);
    }
}
