using System.Collections.Generic;
using System.Drawing;
using Engine;
using NUnit.Framework;

namespace EngineUnitTests
{
    public class MapUnitTests
    {
        [Test]
        public void CheckIfBarrier_InsideBarrier_False()
        {
            var map = new Map(1, 1);

            var result = map.CheckIfBarrier(new Point(1, 1));

            Assert.IsFalse(result);
        }

        [Test]
        public void CheckIfBarrier_OnBarrier_True()
        {
            var map = new Map(1, 1);

            var result = map.CheckIfBarrier(new Point(0, 0));

            Assert.IsTrue(result);
        }

        [Test]
        public void CheckIfBarrier_BehindBarrier_True()
        {
            var map = new Map(1, 1);

            var result = map.CheckIfBarrier(new Point(3, 3));

            Assert.IsTrue(result);
        }

        [Test]
        public void GenerateNewFood_IsPlaceToGenerate_True()
        {
            var map = new Map(1, 1);

            var result = map.GenerateNewFood(new List<Point>());

            Assert.IsTrue(result);
        }
        
        [Test]
        public void GenerateNewFood_NoPlaceToGenerate_False()
        {
            var map = new Map(1, 1);

            var result = map.GenerateNewFood(new List<Point> { new Point(1, 1) });

            Assert.IsFalse(result);
        }

        [Test]
        public void GetMap_MapOfOnePlaceContains_ContainsAll()
        {
            var map = new Map(1, 1);
            var predicted = new List<Point>
            {
                new Point(0,0),
                new Point(0,1),
                new Point(0,2),
                new Point(0,1),
                new Point(2,1),
                new Point(0,2),
                new Point(1,2),
                new Point(2,2),
            };

            var mapPoints = map.Points();
            var result = predicted.TrueForAll(x => mapPoints.Contains(x));

            Assert.IsTrue(result);
        }

        [Test]
        public void GetMap_MapOfOnePlaceNotContains_NotContains()
        {
            var map = new Map(1, 1);
            var predicted = new Point(1, 1);

            var mapPoints = map.Points();
            var result = mapPoints.Contains(predicted);

            Assert.IsFalse(result);
        }

        [Test]
        public void CheckIfFood_IsPlaceToGenerate_True()
        {
            var map = new Map(1, 1);

            map.GenerateNewFood(new List<Point>());
            var result = map.CheckIfFood(new Point(1, 1));

            Assert.IsTrue(result);
        }

        [Test]
        public void CheckIfFood_NoPlaceToGenerate_False()
        {
            var map = new Map(1, 1);

            map.GenerateNewFood(new List<Point> { new Point(1, 1) });
            var result = map.CheckIfFood(new Point(1, 1));

            Assert.IsFalse(result);
        }

        [Test]
        public void CheckIfFood_NoFoodGenerated_False()
        {
            var map = new Map(1, 1);
            var result = map.CheckIfFood(new Point(1, 1));

            Assert.IsFalse(result);
        }
    }
}
