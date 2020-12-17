using System.Collections.Generic;
using System.Drawing;
using Engine;
using Enums.Engine;
using NUnit.Framework;

namespace EngineUnitTests
{
    public class AwatarUnitTests
    {
        [Test]
        public void GetBody_SingleCellBody_AsPredicted()
        {
            var point = new Point(1, 1);
            var awatar = new Avatar(point, Direction.Down);

            var body = awatar.GetBody();
            var resultCount = body.Count;
            var resultContains = body.Contains(point);

            Assert.AreEqual(1, resultCount);
            Assert.IsTrue(resultContains);
        }

        [Test]
        public void Move_OneMoveDownNoPoint_AsPredicted()
        {
            var direction = Direction.Down;
            var awatar = new Avatar(new Point(1, 1), direction);

            awatar.Move(direction);
            awatar.SaveLastMove(false);
            var body = awatar.GetBody();
            var resultCount = body.Count;
            var resultContains = body.Contains(new Point(1, 2));

            Assert.AreEqual(1, resultCount);
            Assert.IsTrue(resultContains);
        }

        [Test]
        public void Move_OneMoveDownPointGained_AsPredicted()
        {
            var direction = Direction.Down;
            var awatar = new Avatar(new Point(1, 1), direction);
            var predicted = new List<Point>
            {
                new Point(1,1),
                new Point(1,2)
            };

            awatar.Move(direction);
            awatar.SaveLastMove(true);
            var body = awatar.GetBody();
            var resultCount = body.Count;
            var resultContains = body.TrueForAll(x => predicted.Contains(x));

            Assert.AreEqual(2, resultCount);
            Assert.IsTrue(resultContains);
        }

        [Test]
        public void CheckIfHit_NotHit_False()
        {
            var direction = Direction.Down;
            var awatar = new Avatar(new Point(1, 1), direction);

            awatar.Move(direction);
            var result = awatar.CheckIfHit();

            Assert.IsFalse(result);
        }

        [Test]
        public void CheckIfHit_HitInFourMoves_True()
        {
            var direction = Direction.Down;
            var awatar = new Avatar(new Point(1, 1), direction);

            awatar.Move(direction);
            awatar.SaveLastMove(true);
            awatar.Move(Direction.Left);
            awatar.SaveLastMove(true);
            awatar.Move(Direction.Up);
            awatar.SaveLastMove(true);
            awatar.Move(Direction.Right);

            var result = awatar.CheckIfHit();

            Assert.IsTrue(result);
        }
    }
}
