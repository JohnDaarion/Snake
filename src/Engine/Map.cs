using Engine.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine
{
    ///<inheritdoc/>
    public class Map : IMap
    {
        private readonly Point _mapMinSize;
        private readonly Point _mapMaxSize;
        private readonly Random _random = new Random();
        private Point _currentFood;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="width">Map width</param>
        /// <param name="hight">Map hight</param>
        public Map(int width, int hight) 
        {

            _mapMinSize = new Point(0, 0);
            _mapMaxSize = new Point(width, hight);
        }

        ///<inheritdoc/>
        public bool CheckIfFood(Point point)
        {
            if (_currentFood == null)
                return false;
            else
                return _currentFood.Equals(point);
        }

        ///<inheritdoc/>
        public bool GenerateNewFood(List<Point> forbiddenLocations)
        {
            if ((_mapMaxSize.X - _mapMinSize.X) * (_mapMaxSize.Y - _mapMinSize.Y) == forbiddenLocations.Count)
                return false;

            _currentFood = GetNewPoint(forbiddenLocations);
            return true;
        }

        ///<inheritdoc/>
        public bool CheckIfBarrier(Point point)
        {
            return point.X <= _mapMinSize.X || point.X >= _mapMaxSize.X + 1 || point.Y <= _mapMinSize.Y || point.Y >= _mapMaxSize.Y + 1;
        }

        ///<inheritdoc/>
        public Point GetFood()
        {
            return _currentFood;
        }

        ///<inheritdoc/>
        public List<Point> GetMap()
        {
            var result = new List<Point>();
            for (var i = _mapMinSize.X; i <= _mapMaxSize.X + 1; i++)
            {
                result.Add(new Point(i, _mapMinSize.Y));
                result.Add(new Point(i, _mapMaxSize.Y + 1));
            }

            for (int i = (int)_mapMinSize.Y; i <= _mapMaxSize.Y; i++)
            {
                result.Add(new Point(_mapMinSize.X, i));
                result.Add(new Point(_mapMaxSize.X + 1, i));
            }

            return result;
        }

        private Point GetNewPoint(List<Point> forbiddenLocations)
        {
            Point point;
            while (true)
            {
                point = new Point(_random.Next(_mapMinSize.X + 1, _mapMaxSize.X + 1), _random.Next(_mapMinSize.Y + 1, _mapMaxSize.Y + 1));
                if (!forbiddenLocations.Contains(point))
                    break;
            }
            return point;
        }
    }
}
