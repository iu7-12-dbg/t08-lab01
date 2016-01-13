using System;
using PathfindingAlgorithms.Cells;

namespace PathfindingAlgorithms.Algorithms.Astar
{
    public class ManhattanHeuristic : IHeuristic
    {
        public double Heuristic(Coordinates a, Coordinates b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}
