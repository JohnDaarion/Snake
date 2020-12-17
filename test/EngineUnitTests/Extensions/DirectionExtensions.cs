using Enums.Engine;
using NUnit.Framework;

namespace EngineUnitTests.Extensions
{
    public class DirectionExtensions
    {
        [Test]
        public void IsDirectionOpposite_IsOpposite_True()
        {
            var resultA = Direction.Down.IsDirectionOpposite(Direction.Up);
            var resultB = Direction.Up.IsDirectionOpposite(Direction.Down);
            var resultC = Direction.Right.IsDirectionOpposite(Direction.Left);
            var resultD = Direction.Left.IsDirectionOpposite(Direction.Right);

            Assert.IsTrue(resultA);
            Assert.IsTrue(resultB);
            Assert.IsTrue(resultC);
            Assert.IsTrue(resultD);
        }

        [Test]
        public void IsDirectionOpposite_IsNotOpposite_False()
        {
            var resultA = Direction.Down.IsDirectionOpposite(Direction.Down);
            var resultB = Direction.Down.IsDirectionOpposite(Direction.Right);
            var resultC = Direction.Down.IsDirectionOpposite(Direction.Left);

            var resultD = Direction.Up.IsDirectionOpposite(Direction.Up);
            var resultE = Direction.Up.IsDirectionOpposite(Direction.Right);
            var resultF = Direction.Up.IsDirectionOpposite(Direction.Left);

            var resultG = Direction.Right.IsDirectionOpposite(Direction.Down);
            var resultH = Direction.Right.IsDirectionOpposite(Direction.Up);
            var resultI = Direction.Right.IsDirectionOpposite(Direction.Right);

            var resultJ = Direction.Left.IsDirectionOpposite(Direction.Down);
            var resultK = Direction.Left.IsDirectionOpposite(Direction.Up);
            var resultL = Direction.Left.IsDirectionOpposite(Direction.Left);

            Assert.IsFalse(resultA);
            Assert.IsFalse(resultB);
            Assert.IsFalse(resultC);
                   
            Assert.IsFalse(resultD);
            Assert.IsFalse(resultE);
            Assert.IsFalse(resultF);
                   
            Assert.IsFalse(resultG);
            Assert.IsFalse(resultH);
            Assert.IsFalse(resultI);
                   
            Assert.IsFalse(resultJ);
            Assert.IsFalse(resultK);
            Assert.IsFalse(resultL);
        }
    }
}
