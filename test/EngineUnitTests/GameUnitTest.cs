using System;
using System.Collections.Generic;
using System.Drawing;
using Engine;
using Engine.Abstractions;
using Enums.Engine;
using NSubstitute;
using NUnit.Framework;

namespace EngineUnitTests
{
    public class GameUnitTest
    {
        private Game _game;
        private IAvatar _avatar;
        private IMap _map;
        private IDownloader _downloader;
        private Action _callback;
        private Action _downloaded;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _avatar = Substitute.For<IAvatar>();
            _map = Substitute.For<IMap>();
            _downloader = Substitute.For<IDownloader>();
            _callback = Substitute.For<Action>();
            _downloaded = Substitute.For<Action>();
        }

        [SetUp]
        public void Setup()
        {
            _game = new Game(_avatar, _map, _downloader, _callback, _downloaded);
        }

        [Test]
        public void SetDirection_UpDirectionNotStarted_DirectionNotSet()
        {
            _game.SetDirection(Direction.Up);
            var result = _game.CurrentDirection;

            Assert.AreEqual(Direction.Down, result);
        }

        [Test]
        public void SetDirection_UpDirectionStopped_DirectionNotSet()
        {
            _game.Start();
            _game.Stop();
            _game.SetDirection(Direction.Up);
            var result = _game.CurrentDirection;

            Assert.AreEqual(Direction.Down, result);
        }

        [Test]
        public void SetDirection_UpDirectionStarted_DirectionSet()
        {
            var direction = Direction.Up;

            _game.Start();
            _game.SetDirection(direction);
            var result = _game.CurrentDirection;

            Assert.AreEqual(direction, result);
        }

        [Test]
        public void SetDirection_SetDirectionTwice_DirectionNotSet()
        {
            var directionA = Direction.Up;
            var directionB = Direction.Left;

            _game.Start();
            _game.SetDirection(directionB);
            _game.SetDirection(directionA);
            var result = _game.CurrentDirection;

            Assert.Multiple(() =>
            {
                Assert.AreNotEqual(directionA, result);
                Assert.AreEqual(directionB, result);
            });
        }

        [Test]
        public void GetView_CorrectData_AsPredicted()
        {
            var awatar = Substitute.For<IAvatar>();
            awatar.Body().ReturnsForAnyArgs(new List<Point> { new Point(1,1) });
            var mapPoints = new List<Point>
            {
                new Point(0,0),
                new Point(0,1),
                new Point(0,2),
                new Point(0,3),
                new Point(3,0),
                new Point(3,1),
                new Point(3,2),
                new Point(3,3),
                new Point(1,0),
                new Point(2,0),
                new Point(1,3),
                new Point(2,3)
            };
            var map = Substitute.For<IMap>();
            map.Points().ReturnsForAnyArgs(mapPoints);
            map.Food().ReturnsForAnyArgs(new Point(2, 2));

            var game = new Game(awatar, map, _downloader, _callback, _downloaded);

            var predicted = new FieldType[4, 4];
            predicted[0, 0] = FieldType.Wall;
            predicted[0, 1] = FieldType.Wall;
            predicted[0, 2] = FieldType.Wall;
            predicted[0, 3] = FieldType.Wall;
            predicted[3, 0] = FieldType.Wall;
            predicted[3, 1] = FieldType.Wall;
            predicted[3, 2] = FieldType.Wall;
            predicted[3, 3] = FieldType.Wall;
            predicted[1, 0] = FieldType.Wall;
            predicted[2, 0] = FieldType.Wall;
            predicted[1, 3] = FieldType.Wall;
            predicted[2, 3] = FieldType.Wall;
            predicted[1, 1] = FieldType.Body;
            predicted[2, 2] = FieldType.Food;

            var view = game.GetView();
            var result = true;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (predicted[i, j] != view[i, j])
                        result = false;
                }
            }

            Assert.Multiple(() =>
            {
                Assert.AreEqual(predicted.Length, view.Length);
                Assert.IsTrue(result);
            });
        }
    }
}
