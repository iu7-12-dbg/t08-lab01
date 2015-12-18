using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathfindingAlgorithms.Cells;

namespace PathfindingAlgorithms.Algorithms
{
    public class Dijkstra : IPathFindingAlgorithm
    {
        protected class DNode
        {
            public DNode()
            {
                val = 1.0 / 0.0;
                prev = null;
                visited = false;
            }
            public double val;
            public bool visited;
            public Coordinates? prev;
        }

        protected void CheckForeign(ICell[,] CM, DNode[,] D, Coordinates coords)
        {
            int f = coords.X, s = coords.Y;
            double w;
            //left
            if ((f > 0) && (CM[f - 1, s].Weight >= 0))
            {
                w = CM[f - 1, s].Weight + D[f, s].val;
                if (w < D[f - 1, s].val)
                {
                    D[f - 1, s].val = w;
                    D[f - 1, s].prev = coords;
                }
            }
            //right
            if ((f < CM.GetLength(0) - 1) && (CM[f + 1, s].Weight >= 0))
            {
                w = CM[f + 1, s].Weight + D[f, s].val;
                if (w < D[f + 1, s].val)
                {
                    D[f + 1, s].val = w;
                    D[f + 1, s].prev = coords;
                }
            }
            //top
            if ((s > 0) && (CM[f, s - 1].Weight >= 0))
            {
                w = CM[f, s - 1].Weight + D[f, s].val;
                if (w < D[f, s - 1].val)
                {
                    D[f, s - 1].val = w;
                    D[f, s - 1].prev = coords;
                }
            }
            //bottom
            if ((s < CM.GetLength(1) - 1) && (CM[f, s + 1].Weight >= 0))
            {
                w = CM[f, s + 1].Weight + D[f, s].val;
                if (w < D[f, s + 1].val)
                {
                    D[f, s + 1].val = w;
                    D[f, s + 1].prev = coords;
                }
            }
        }

        protected Coordinates? FindMin(DNode[,] D)
        {
            bool foundfirst = false;
            Coordinates? min = null;
            for (int i = 0; (i < D.GetLength(0)) && !foundfirst; i++)
                for (int j = 0; (j < D.GetLength(1)) && !foundfirst; j++)
                    if (!D[i, j].visited)
                    {
                        foundfirst = true;
                        min = new Coordinates(i, j);
                    }
            if (min.HasValue)
                for (int i = 0; i < D.GetLength(0); i++)
                    for (int j = 0; j < D.GetLength(1); j++)
                        if ((!D[i, j].visited) && (D[i, j].val < D[min.Value.X, min.Value.Y].val))
                            min = new Coordinates(i, j);
            return min;
        }

        public IEnumerable<ICell> Process(ICell[,] CellMap, Coordinates start, Coordinates end)
        {
            if (start.X < 0 || start.Y < 0 || start.X >= CellMap.GetLength(0) || start.Y >= CellMap.GetLength(1))
                throw new ArgumentOutOfRangeException("start");
            if (end.X < 0 || end.Y < 0 || end.X >= CellMap.GetLength(0) || end.Y >= CellMap.GetLength(1))
                throw new ArgumentOutOfRangeException("end");

            DNode[,] dist = new DNode[CellMap.GetLength(0), CellMap.GetLength(1)];

            int count = CellMap.GetLength(0) * CellMap.GetLength(1);
            for (int i = 0; i < CellMap.GetLength(0); ++i)
                for (int j = 0; j < CellMap.GetLength(1); ++j)
                    dist[i, j] = new DNode();

            dist[start.X, start.Y].val = 0;

            Coordinates? min = null;
            do
            {
                min = FindMin(dist);
                if (min.HasValue)
                {
                    CheckForeign(CellMap, dist, min.Value);
                    dist[min.Value.X, min.Value.Y].visited = true;
                }
            } while (min.HasValue);


            int waylen = 0;
            DNode dnode = dist[end.X, end.Y];
            while (dnode.prev.HasValue)
            {
                ++waylen;
                dnode = dist[dnode.prev.Value.X, dnode.prev.Value.Y];
            }

            if (waylen > 0)
            {
                ICell[] res = new ICell[waylen + 1];
                res[waylen] = CellMap[end.X, end.Y];
                int i = waylen - 1;
                dnode = dist[end.X, end.Y];
                while (dnode.prev.HasValue)
                {
                    res[i--] = CellMap[dnode.prev.Value.X, dnode.prev.Value.Y];
                    dnode = dist[dnode.prev.Value.X, dnode.prev.Value.Y];
                }
                return res;
            }
            else
            {
                return new LinkedList<ICell>();
            }
        }
    }
}
