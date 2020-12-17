using Enums.Engine;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.Abstractions
{
    /// <summary>
    /// Snake avatar
    /// </summary>
    public interface IAvatar
    {
        /// <summary>
        /// Move avatar
        /// </summary>
        /// <param name="direction">Move direction</param>
        /// <returns>Location after move</returns>
        Point Move(Direction? direction);

        /// <summary>
        /// Save last calculated move
        /// </summary>
        /// <param name="pointGained">Was point gained in that move</param>
        void SaveLastMove(bool pointGained);

        /// <summary>
        /// Check if avatar hit himself
        /// </summary>
        /// <returns></returns>
        bool CheckIfHit();

        /// <summary>
        /// Get body points
        /// </summary>
        /// <returns>Body points</returns>
        List<Point> GetBody();
    }
}
