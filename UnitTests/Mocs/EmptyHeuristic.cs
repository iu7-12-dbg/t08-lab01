using PathfindingAlgorithms.Algorithms.Astar;
using PathfindingAlgorithms.Cells;

namespace UnitTests.Mocs
{
    class EmptyHeuristic : IHeuristic
    {
        public double Heuristic(Coordinates a, Coordinates b)
        {
            return 0;
        }
    }
}
