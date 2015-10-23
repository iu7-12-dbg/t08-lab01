﻿using PathfindingAlgorithms.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms
{
    public interface IPathFindingAlgorithm
    {
        IEnumerable<ICell> Process(IList<IList<ICell>> CellMap, Tuple<int,int> start, Tuple<int, int> end);
    }
}