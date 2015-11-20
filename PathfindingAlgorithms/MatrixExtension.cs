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
        /// <summary>
        /// Returns maximum bottom-right coordinate of given matix. Returns -1 if rows or columns are empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Coordinates MaxCoordinate<T>(this IList<IList<T>> matrix)
        {
            var x = matrix.Count - 1;
            var y = (x != -1) ? matrix[x].Count - 1 : -1;
            return new Coordinates(x, y);
        }
    }
}
