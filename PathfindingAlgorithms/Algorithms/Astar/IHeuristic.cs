using PathfindingAlgorithms.Cells;

namespace PathfindingAlgorithms.Algorithms.Astar
{
    public interface IHeuristic
    {
        double Heuristic(Coordinates a, Coordinates b);
    }
}
