using System;
using System.Text;

namespace PathfindingAlgorithms.Cells
{
    public struct Coordinates : IEquatable<Coordinates>
    { 
        public int X { get; }

        public int Y { get; }

        public Coordinates(int x, int y)
			: this()
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

        public override bool Equals(object obj)
        {
            var other = obj as Coordinates?;
            if (!other.HasValue)
                return false;
            else
                return Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public bool Equals(Coordinates other)
        {
            return X == other.X && Y == other.Y;
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

		public static bool operator==(Coordinates a, Coordinates b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator!=(Coordinates a, Coordinates b)
		{
			return a.X != b.X || a.Y != b.Y;
		}

        public override string ToString()
        {
            return $"X = {X}, Y = {Y}";
        }
    }
}
