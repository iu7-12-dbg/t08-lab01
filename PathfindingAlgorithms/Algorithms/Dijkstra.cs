using System.Collections.Generic;
using PathfindingAlgorithms.Cells;
using System.Diagnostics.Contracts;

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

        protected void CheckForeign(ICell[,] cm, DNode[,] d, Coordinates coords)
        {
            int f = coords.X, s = coords.Y;
            double w;
            //left
            if ((f > 0) && (cm[f - 1, s].Weight >= 0))
            {
                w = cm[f - 1, s].Weight + d[f, s].val;
                if (w < d[f - 1, s].val)
                {
                    d[f - 1, s].val = w;
                    d[f - 1, s].prev = coords;
                }
            }
            //right
            if ((f < cm.GetLength(0) - 1) && (cm[f + 1, s].Weight >= 0))
            {
                w = cm[f + 1, s].Weight + d[f, s].val;
                if (w < d[f + 1, s].val)
                {
                    d[f + 1, s].val = w;
                    d[f + 1, s].prev = coords;
                }
            }
            //top
            if ((s > 0) && (cm[f, s - 1].Weight >= 0))
            {
                w = cm[f, s - 1].Weight + d[f, s].val;
                if (w < d[f, s - 1].val)
                {
                    d[f, s - 1].val = w;
                    d[f, s - 1].prev = coords;
                }
            }
            //bottom
            if ((s < cm.GetLength(1) - 1) && (cm[f, s + 1].Weight >= 0))
            {
                w = cm[f, s + 1].Weight + d[f, s].val;
                if (w < d[f, s + 1].val)
                {
                    d[f, s + 1].val = w;
                    d[f, s + 1].prev = coords;
                }
            }
        }

        protected Coordinates? FindMin(DNode[,] d)
        {
            bool foundfirst = false;
            Coordinates? min = null;
            for (int i = 0; (i < d.GetLength(0)) && !foundfirst; i++)
                for (int j = 0; (j < d.GetLength(1)) && !foundfirst; j++)
                    if (!d[i, j].visited)
                    {
                        foundfirst = true;
                        min = new Coordinates(i, j);
                    }
            if (min.HasValue)
                for (int i = 0; i < d.GetLength(0); i++)
                    for (int j = 0; j < d.GetLength(1); j++)
                        if ((!d[i, j].visited) && (d[i, j].val < d[min.Value.X, min.Value.Y].val))
                            min = new Coordinates(i, j);
            return min;
        }

        public IEnumerable<ICell> Process(ICell[,] cellMap, Coordinates start, Coordinates end)
        {
            Contract.Requires(cellMap != null);
            Contract.Requires(start.Inside(new Coordinates(cellMap.GetLength(0), cellMap.GetLength(1))));
            Contract.Requires(end.Inside(new Coordinates(cellMap.GetLength(0), cellMap.GetLength(1))));

            DNode[,] dist = new DNode[cellMap.GetLength(0), cellMap.GetLength(1)];

            int count = cellMap.GetLength(0) * cellMap.GetLength(1);
            for (int i = 0; i < cellMap.GetLength(0); ++i)
                for (int j = 0; j < cellMap.GetLength(1); ++j)
                    dist[i, j] = new DNode();

            dist[start.X, start.Y].val = 0;

            Coordinates? min = null;
            do
            {
                min = FindMin(dist);
                if (min.HasValue)
                {
                    CheckForeign(cellMap, dist, min.Value);
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
                res[waylen] = cellMap[end.X, end.Y];
                int i = waylen - 1;
                dnode = dist[end.X, end.Y];
                while (dnode.prev.HasValue)
                {
                    res[i--] = cellMap[dnode.prev.Value.X, dnode.prev.Value.Y];
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
