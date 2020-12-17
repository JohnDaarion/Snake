namespace Enums.Engine
{
    /// <summary>
    /// <see cref="Direction"/> extensions
    /// </summary>
    public static class DirectionExtensions
    {
        /// <summary>
        /// Is checked direction opposite to base
        /// </summary>
        /// <param name="baseDirection">Base direction</param>
        /// <param name="checkedDirection">Checked direction</param>
        /// <returns>Is cheched direction opposite to base</returns>
        public static bool IsDirectionOpposite(this Direction baseDirection, Direction checkedDirection)
        {
            switch (baseDirection) 
            {
                case Direction.Up:
                    return checkedDirection == Direction.Down;

                case Direction.Down:
                    return checkedDirection == Direction.Up;

                case Direction.Left:
                    return checkedDirection == Direction.Right;

                case Direction.Right:
                    return checkedDirection == Direction.Left;

                default: 
                    return false;
            }
        }
    }
}
