using Engine.Abstractions;
using Enums.Engine;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Engine
{
    ///<inheritdoc/>
    public class Avatar : IAvatar
    {
        private List<Point> _body = new List<Point>();
        private Point _currentPoint;
        private Direction _currentDirection;
        private readonly int _step;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="startingPoint">Avatar's starting point</param>
        /// <param name="startingDirection">Avatar's starting position</param>
        /// <param name="step">How big step should be (in fields)</param>
        public Avatar(Point startingPoint, Direction startingDirection, int step = 1) 
        {
            _step = step;
            _body.Add(startingPoint);
            _currentDirection = startingDirection;
        }

        ///<inheritdoc/>
        public Point Move(Direction? direction) 
        {
            Point current = _body.Last();

            if (direction == null || (direction.Value.IsDirectionOpposite(_currentDirection) && _body.Count != 1))
                direction = _currentDirection;

            _currentPoint = direction switch
            {
                Direction.Up => new Point(current.X, current.Y - _step),
                Direction.Down => new Point(current.X, current.Y + _step),
                Direction.Left => new Point(current.X - _step, current.Y),
                Direction.Right => new Point(current.X + _step, current.Y),
                _ => current
            };

            _currentDirection = direction.Value;
            return _currentPoint;
        }

        ///<inheritdoc/>
        public bool CheckIfHit()
        {
            return _body.Contains(_currentPoint);
        }

        ///<inheritdoc/>
        public void SaveLastMove(bool pointGained)
        {
            _body.Add(_currentPoint);
            if (!pointGained)
                _body.RemoveAt(0);
        }

        ///<inheritdoc/>
        public List<Point> Body()
        {
            return _body;
        }
    }
}
