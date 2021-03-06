using PathfindingAlgorithms.Algorithms.Astar;
using System;
using Microsoft.Pex.Framework;

namespace PathfindingAlgorithms.Algorithms.Astar
{
    /// <summary>A factory for PathfindingAlgorithms.Algorithms.Astar.Astar instances</summary>
    public static partial class AstarFactory
    {
        /// <summary>A factory for PathfindingAlgorithms.Algorithms.Astar.Astar instances</summary>
        [PexFactoryMethod(typeof(global::PathfindingAlgorithms.Algorithms.Astar.Astar))]
        public static global::PathfindingAlgorithms.Algorithms.Astar.Astar Create(IHeuristic heuristic_iHeuristic, IAdjacement adjacement_iAdjacement)
        {
            global::PathfindingAlgorithms.Algorithms.Astar.Astar astar
               = new global::PathfindingAlgorithms.Algorithms.Astar.Astar
                     (heuristic_iHeuristic, adjacement_iAdjacement);
            return astar;

            // TODO: Edit factory method of Astar
            // This method should be able to configure the object in all possible ways.
            // Add as many parameters as needed,
            // and assign their values to each field by using the API.
        }
    }
}
