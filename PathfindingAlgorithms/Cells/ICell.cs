namespace PathfindingAlgorithms.Cells
{
    public interface ICell
    {
        double Weight { get; set; }
        
        Coordinates Coordinates { get; }
    }
}
