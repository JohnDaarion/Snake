using Enums.Engine;
using NUnit.Framework;

namespace EngineUnitTests.Extensions
{
    public class DirectionExtensions
    {
        [TestCase(Direction.Down, Direction.Up)]
        [TestCase(Direction.Up, Direction.Down)]
        [TestCase(Direction.Right, Direction.Left)]
        [TestCase(Direction.Left, Direction.Right)]
        public void IsDirectionOpposite_IsOpposite_True(Direction directionA, Direction directionB)
        {
            var result = directionA.IsDirectionOpposite(directionB);

            Assert.IsTrue(result);
        }

        [TestCase(Direction.Down)]
        [TestCase(Direction.Down)]
        [TestCase(Direction.Left)]
        public void IsDirectionOpposite_IsNotOpposite_False(Direction direction)
        {
            var result = Direction.Down.IsDirectionOpposite(direction);

            Assert.IsFalse(result);
        }
    }
}
