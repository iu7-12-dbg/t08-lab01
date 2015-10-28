using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingAlgorithms.Cells
{
    public struct Coordinates
    { 
        public int X { get; set; }

        public int Y { get; set; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Checks if point is inside rectangle between (0, 0) and max (including borders). Doesn't validate parameters.
        /// </summary>
        /// <param name="max">Bottom-right corner of the rectangle.</param>
        /// <returns></returns>
        public bool Inside(Coordinates max)
        {
            return Inside(new Coordinates(), max);
        }

        /// <summary>
        /// Checks if point is inside rectangle between a and b (including borders). Doesn't validate parameters.
        /// </summary>
        /// <param name="min">Upper-left corner of the rectangle.</param>
        /// <param name="max">Bottom-right corner of the rectangle.</param>
        /// <returns></returns>
        public bool Inside(Coordinates min, Coordinates max)
        {
            return this >= min && this <= max;
        }

        public static bool operator<(Coordinates a, Coordinates b)
        {
            return a.X < b.X && a.Y < b.Y;
        }

        public static bool operator>(Coordinates a, Coordinates b)
        {
            return a.X > b.X && a.Y > b.Y;
        }

        public static bool operator<=(Coordinates a, Coordinates b)
        {
            return a.X <= b.X && a.Y <= b.Y;
        }

        public static bool operator>=(Coordinates a, Coordinates b)
        {
            return a.X >= b.X && a.Y >= b.Y;
        }
    }
}
