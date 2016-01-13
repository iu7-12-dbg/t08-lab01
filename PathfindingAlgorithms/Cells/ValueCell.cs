namespace PathfindingAlgorithms.Cells
{
    public class ValueCell : ICell
    {
        public ValueCell(Coordinates coordinates)
            : this(coordinates, 0)
        {
        }

        public ValueCell(Coordinates coordinates, double weight)
        {
            this.Coordinates = coordinates;
            this.Weight = weight;
        }

        public Coordinates Coordinates { get; }

        public double Weight { get; set; }
    }
}
