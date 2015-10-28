using PathfindingAlgorithms.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingAlgorithms
{
    public static class MatrixExtension
    {
        public static Coordinates Size<T>(this IList<IList<T>> matrix)
        {
            var x = matrix.Count;
            var y = matrix[x - 1].Count;
            return new Coordinates(x, y);
        }
    }
}
