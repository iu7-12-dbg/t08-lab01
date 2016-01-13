using System.Collections.Generic;
using PathfindingAlgorithms.Cells;

namespace PathfindingAlgorithms.Algorithms.Astar
{
    public class StraightAdjacement : IAdjacement
    {
        public IEnumerable<Coordinates> Adjacement(ICell[,] grid, Coordinates at)
        {
            var res = new List<Coordinates>(4);

            Coordinates? c;
            if ((c = TestNode(grid, at.X, at.Y - 1)).HasValue)
                res.Add(c.Value);
            if ((c = TestNode(grid, at.X - 1, at.Y)).HasValue)
                res.Add(c.Value);
            if ((c = TestNode(grid, at.X, at.Y + 1)).HasValue)
                res.Add(c.Value);
            if ((c = TestNode(grid, at.X + 1, at.Y)).HasValue)
                res.Add(c.Value);

            return res;
        }

        private static Coordinates? TestNode(ICell[,] grid, int x, int y)
        {
            int xsize = grid.GetLength(0), ysize = grid.GetLength(1);
            if (x >= 0 && x < xsize && y >= 0 && y < ysize && grid[x, y].Weight >= 0 && grid[x, y].Weight < 1)
                return grid[x, y].Coordinates;
            return null;
        }
    }
}
