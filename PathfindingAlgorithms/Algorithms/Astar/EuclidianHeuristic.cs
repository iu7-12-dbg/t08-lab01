using System;
using PathfindingAlgorithms.Cells;

namespace PathfindingAlgorithms.Algorithms.Astar
{
    public class EuclidianHeuristic : IHeuristic
    {
        public double Heuristic(Coordinates a, Coordinates b)
        {
            int diffX = a.X - b.X;
            int diffY = a.Y - b.Y;
            return Math.Sqrt(diffX * diffX + diffY * diffY);
        }
    }
}
