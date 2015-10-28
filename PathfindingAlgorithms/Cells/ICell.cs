using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingAlgorithms.Cells
{
    public interface ICell
    {
        double Weight { get; set; }
        
        Coordinates Coordinates { get; }
    }
}
