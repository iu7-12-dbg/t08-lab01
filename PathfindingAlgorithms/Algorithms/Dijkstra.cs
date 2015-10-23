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
            public Tuple<int, int> prev;
        }

        protected void CheckForeign(IList<IList<ICell>> CM, DNode[][] D, int f, int s)
        {
            double w;
            //top
            if (f > 0)
            {
                w = CM[f - 1][s].Weight + D[f][s].val;
                if (w < D[f - 1][s].val)
                {
                    D[f - 1][s].val = w;
                    D[f - 1][s].prev = new Tuple<int, int>(f, s);
                }
            }
            //bottom
            if (f < CM.Count - 1)
            {
                w = CM[f + 1][s].Weight + D[f][s].val;
                if (w < D[f + 1][s].val)
                {
                    D[f + 1][s].val = w;
                    D[f + 1][s].prev = new Tuple<int, int>(f, s);
                }
            }
            //left
            if (s > 0)
            {
                w = CM[f][s - 1].Weight + D[f][s].val;
                if (w < D[f][s - 1].val)
                {
                    D[f][s - 1].val = w;
                    D[f][s - 1].prev = new Tuple<int, int>(f, s);
                }
            }
            //right
            if (s < CM[f].Count - 1)
            {
                w = CM[f][s + 1].Weight + D[f][s].val;
                if (w < D[f][s + 1].val)
                {
                    D[f][s + 1].val = w;
                    D[f][s + 1].prev = new Tuple<int, int>(f, s);
                }
            }
        }

        public IEnumerable<ICell> Process(IList<IList<ICell>> CellMap, Tuple<int, int> start, Tuple<int, int> end)
        {
            if (start.Item1 < 0 || start.Item2 < 0 || start.Item1 >= CellMap.Count || start.Item2 >= CellMap[start.Item1].Count)
                throw new ArgumentOutOfRangeException("start");
            if (end.Item1 < 0 || end.Item2 < 0 || end.Item1 >= CellMap.Count || end.Item2 >= CellMap[end.Item1].Count)
                throw new ArgumentOutOfRangeException("end");

            DNode[][] dist = new DNode[CellMap.Count][];

            int count = 0;
            for (int i = 0; i < CellMap.Count; ++i)
            {
                dist[i] = new DNode[CellMap[i].Count];
                count += CellMap[i].Count;
                for (int j = 0; j < CellMap[i].Count; ++j)
                {
                    dist[i][j] = new DNode();
                    dist[i][j].val = 1.0 / 0.0;
                }

            }

            while (count-- > 0)
            {
                Tuple<int, int> min = new Tuple<int, int>(0, 0);
                for (int i = 0; i < dist.Length; ++i)
                    for (int j = 0; j < dist[i].Length; ++j)
                    {
                        if (dist[i][j].val < dist[min.Item1][min.Item2].val)
                            min = new Tuple<int, int>(i, j);
                    }
                CheckForeign(CellMap, dist, min.Item1, min.Item2);
            }

            int waylen = 0;
            DNode dnode = dist[end.Item1][end.Item2];
            while (dnode.prev != null)
            {
                ++waylen;
                dnode = dist[dnode.prev.Item1][dnode.prev.Item2];
            }

            if (waylen > 0)
            {
                ICell[] res = new ICell[waylen];
                res[waylen - 1] = CellMap[end.Item1][end.Item2];
                int i = waylen - 2;
                dnode = dist[end.Item1][end.Item2];
                while (dnode.prev != null)
                {
                    res[i--] = CellMap[dnode.prev.Item1][dnode.prev.Item2];
                    dnode = dist[dnode.prev.Item1][dnode.prev.Item2];
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
