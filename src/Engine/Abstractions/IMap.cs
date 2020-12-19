using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Engine.Abstractions
{
    /// <summary>
    /// Map
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Check if checked point is in current food location
        /// </summary>
        /// <param name="point">Checked point</param>
        /// <returns>If checked point is in current food location</returns>
        bool CheckIfFood(Point point);

        /// <summary>
        /// Generate new food
        /// </summary>
        /// <param name="forbiddenLocations">List of forbidden locations</param>
        /// <returns>Was point created</returns>
        bool GenerateNewFood(List<Point> forbiddenLocations);

        /// <summary>
        /// Checks if requested point is on barrier
        /// </summary>
        /// <param name="point">Requested point</param>
        /// <returns>If requested point is on barrier</returns>
        bool CheckIfBarrier(Point point);

        /// <summary>
        /// Get current food's location
        /// </summary>
        /// <returns>Current food's location</returns>
        Point Food();

        /// <summary>
        /// Gets list of map points
        /// </summary>
        /// <returns>List of map points</returns>
        List<Point> Points();
    }
}
