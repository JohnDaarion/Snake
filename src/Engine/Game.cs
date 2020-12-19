using Engine.Abstractions;
using Enums.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

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

        /// <summary>
        /// Current direction
        /// </summary>
        public BlockingCollection<string> Downloaded { get; private set; } = new BlockingCollection<string>();

        private readonly IAvatar _currentAwatar;
        private readonly IMap _currentMap;
        private readonly IDownloader _downloader;
        private readonly Action _callback;
        private readonly Action _downloaded;
        private readonly TimeSpan _speed = TimeSpan.FromMilliseconds(250);
        private readonly Timer _timer;
        private bool _canSetDirection;
        private readonly CancellationTokenSource _source = new CancellationTokenSource();
        private readonly CancellationToken _token;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="callback">Action to execute when view changes</param>
        public Game(IAvatar currentAwatar, IMap currentMap, IDownloader downloader, Action callback, Action downloaded)
        {
            _callback = callback;
            CurrentDirection = Direction.Down;
            _currentAwatar = currentAwatar;
            _currentMap = currentMap;
            _downloader = downloader;
            _downloaded = downloaded;
            _timer = new Timer(_speed.TotalMilliseconds);
            _timer.Elapsed += OnTimedEvent;
            _token = _source.Token;
        }

        /// <summary>
        /// Start game
        /// </summary>
        public void Start()
        {
            _canSetDirection = true;
            _currentMap.GenerateNewFood(_currentAwatar.Body());
            _timer.Start();
        }

        /// <summary>
        /// Stop game
        /// </summary>
        public void Stop()
        {
            _canSetDirection = false;
            _timer.Stop();
            _source.Cancel();
        }

        /// <summary>
        /// Set new direction
        /// </summary>
        /// <param name="direction">New direction</param>
        public void SetDirection(Direction direction)
        {
            if (_canSetDirection)
            {
                CurrentDirection = direction;
                _canSetDirection = false;
            }
        }

        /// <summary>
        /// Get view in array form
        /// </summary>
        /// <returns>Array of field types on corresponding position</returns>
        public FieldType[,] GetView()
        {     
            var map = _currentMap.Points();
            var body = _currentAwatar.Body();
            var food = _currentMap.Food();
            var result = new FieldType[map.Max(point => point.X) + 1, map.Max(point => point.Y) + 1];

            map.ForEach(point => result[point.X, point.Y] = FieldType.Wall);
            body.ForEach(point => result[point.X, point.Y] = FieldType.Body);

            if (food != null)
                result[food.X, food.Y] = FieldType.Food;

            return result;
        }

        /// <summary>
        /// Get map view in array form
        /// </summary>
        /// <returns>Array of field types on corresponding position</returns>
        public List<Point> Map() => _currentMap.Points();

        /// <summary>
        /// Get food
        /// </summary>
        /// <returns>Current food position</returns>
        public Point Food() => _currentMap.Food();

        /// <summary>
        /// Get view in array form
        /// </summary>
        /// <returns>List of body positions</returns>
        public List<Point> Body() => _currentAwatar.Body();

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            GameStatus = Move(CurrentDirection);
            OnChange();
        }

        private Status Move(Direction direction) 
        {
            var checkPoint = _currentAwatar.Move(direction);
            _canSetDirection = true;

            if (_currentMap.CheckIfBarrier(checkPoint) || _currentAwatar.CheckIfHit())
                return Status.Lost;

            if (_currentMap.CheckIfFood(checkPoint))
            {
                AddPoint();
                _currentAwatar.SaveLastMove(true);
                if (!_currentMap.GenerateNewFood(_currentAwatar.Body()))
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
            Score++;
            _downloader.DownloadNextPicture(Downloaded, _downloaded, _token);
        }

        private void OnChange()
        {
            _callback.Invoke();
        }     
    }
}
