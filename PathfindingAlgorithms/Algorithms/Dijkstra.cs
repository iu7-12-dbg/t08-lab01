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
        protected class DNode
        {
            public DNode()
            {
                val = 1.0 / 0.0;
                prev = null;
            }
            public double val;
            public Coordinates? prev;
        }

        protected void CheckForeign(ICell[,] CM, DNode[,] D, int f, int s)
        {
            double w;
            //top
            if (f > 0)
            {
                w = CM[f - 1, s].Weight + D[f, s].val;
                if (w < D[f - 1, s].val)
                {
                    D[f - 1, s].val = w;
                    D[f - 1, s].prev = new Coordinates(f, s);
                }
            }
            //bottom
            if (f < CM.GetLength(0) - 1)
            {
                w = CM[f + 1, s].Weight + D[f, s].val;
                if (w < D[f + 1, s].val)
                {
                    D[f + 1, s].val = w;
                    D[f + 1, s].prev = new Coordinates(f, s);
                }
            }
            //left
            if (s > 0)
            {
                w = CM[f, s - 1].Weight + D[f, s].val;
                if (w < D[f, s - 1].val)
                {
                    D[f, s - 1].val = w;
                    D[f, s - 1].prev = new Coordinates(f, s);
                }
            }
            //right
            if (s < CM.GetLength(1) - 1)
            {
                w = CM[f, s + 1].Weight + D[f, s].val;
                if (w < D[f, s + 1].val)
                {
                    D[f, s + 1].val = w;
                    D[f, s + 1].prev = new Coordinates(f, s);
                }
            }
        }

        public IEnumerable<ICell> Process(ICell[,] CellMap, Coordinates start, Coordinates end)
        {
            if (start.X < 0 || start.Y < 0 || start.X >= CellMap.GetLength(0) || start.Y >= CellMap.GetLength(1))
                throw new ArgumentOutOfRangeException("start");
            if (end.X < 0 || end.Y < 0 || end.X >= CellMap.GetLength(0) || end.Y >= CellMap.GetLength(1))
                throw new ArgumentOutOfRangeException("end");

            DNode[,] dist = new DNode[CellMap.GetLength(0), CellMap.GetLength(1)];

            int count = 0;
            for (int i = 0; i < CellMap.GetLength(0); ++i)
            {
                count += CellMap.GetLength(1);
                for (int j = 0; j < CellMap.GetLength(1); ++j)
                {
                    dist[i, j] = new DNode();
                    dist[i, j].val = 1.0 / 0.0;
                }

            }

            while (count-- > 0)
            {
                Coordinates min = new Coordinates(0, 0);
                for (int i = 0; i < dist.Length; ++i)
                    for (int j = 0; j < dist.GetLength(1); ++j)
                    {
                        if (dist[i, j].val < dist[min.X, min.Y].val)
                            min = new Coordinates(i, j);
                    }
                CheckForeign(CellMap, dist, min.X, min.Y);
            }

            int waylen = 0;
            DNode dnode = dist[end.X, end.Y];
            while (dnode.prev.HasValue)
            {
                ++waylen;
                dnode = dist[dnode.prev.Value.X, dnode.prev.Value.Y];
            }

            if (waylen > 0)
            {
                ICell[] res = new ICell[waylen];
                res[waylen - 1] = CellMap[end.X, end.Y];
                int i = waylen - 2;
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
                return null;
            }
        }
    }
}
