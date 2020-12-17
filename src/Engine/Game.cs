using Engine.Abstractions;
using Enums.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;

namespace Engine
{
    /// <summary>
    /// Base game
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Game status
        /// </summary>
        public Status GameStatus { get; private set; } = Status.Ready;

        /// <summary>
        /// Game score
        /// </summary>
        public int Score { get; private set; } = 0;

        /// <summary>
        /// Current direction
        /// </summary>
        public Direction CurrentDirection { get; private set; } = Direction.Down;

        private readonly IAvatar _currentAwatar;
        private readonly IMap _currentMap;
        private readonly IDownloader _downloader;
        private readonly Action _callback;
        private readonly TimeSpan speed = TimeSpan.FromMilliseconds(250);
        private readonly Timer timer;
        private bool canSetDirection;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="callback">Action to execute when view changes</param>
        public Game(IAvatar currentAwatar, IMap currentMap, IDownloader downloader, Action callback)
        {
            _callback = callback;
            CurrentDirection = Direction.Down;
            _currentAwatar = currentAwatar;
            _currentMap = currentMap;
            _downloader = downloader;
            timer = new Timer(speed.TotalMilliseconds);
            timer.Elapsed += OnTimedEvent;
        }

        /// <summary>
        /// Start game
        /// </summary>
        public void Start()
        {
            canSetDirection = true;
            _currentMap.GenerateNewFood(_currentAwatar.GetBody());
            timer.Start();
        }

        /// <summary>
        /// Stop game
        /// </summary>
        public void Stop()
        {
            canSetDirection = false;
            timer.Stop();
        }

        /// <summary>
        /// Set new direction
        /// </summary>
        /// <param name="direction">New direction</param>
        public void SetDirection(Direction direction)
        {
            if (canSetDirection)
            {
                CurrentDirection = direction;
                canSetDirection = false;
            }
        }

        /// <summary>
        /// Get view in array form
        /// </summary>
        /// <returns>Array of field types on corresponding position</returns>
        public FieldType[,] GetView()
        {     
            var map = _currentMap.GetMap();
            var body = _currentAwatar.GetBody();
            var food = _currentMap.GetFood();
            var result = new FieldType[map.Max(point => point.X) + 1, map.Max(point => point.Y) + 1];

            map.ForEach(point => result[point.X, point.Y] = FieldType.Wall);
            body.ForEach(point => result[point.X, point.Y] = FieldType.Body);

            if (food != null)
            {
                result[food.X, food.Y] = FieldType.Food;
            }

            return result;
        }

        /// <summary>
        /// Get map view in array form
        /// </summary>
        /// <returns>Array of field types on corresponding position</returns>
        public List<Point> GetMap()
        {
            return _currentMap.GetMap();
        }

        /// <summary>
        /// Get food
        /// </summary>
        /// <returns>Current food position</returns>
        public Point GetFood()
        {
            return _currentMap.GetFood();
        }

        /// <summary>
        /// Get view in array form
        /// </summary>
        /// <returns>List of body positions</returns>
        public List<Point> GetBody()
        {
            return _currentAwatar.GetBody();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            GameStatus = Move(CurrentDirection);
            OnChange();
        }

        private Status Move(Direction direction) 
        {
            var checkPoint = _currentAwatar.Move(direction);
            canSetDirection = true;

            if (_currentMap.CheckIfBarrier(checkPoint) || _currentAwatar.CheckIfHit())
                return Status.Lost;

            if (_currentMap.CheckIfFood(checkPoint))
            {
                AddPoint();
                _currentAwatar.SaveLastMove(true);
                if (!_currentMap.GenerateNewFood(_currentAwatar.GetBody()))
                    return Status.Win;

                return Status.PointGained;
            }
            else
            {
                _currentAwatar.SaveLastMove(false);
                return Status.Ok;
            }
        }

        private void AddPoint()
        {
            _downloader.DownloadNextPicture();
            Score++;
        }

        private void OnChange()
        {
            _callback.Invoke();
        }     
    }
}
