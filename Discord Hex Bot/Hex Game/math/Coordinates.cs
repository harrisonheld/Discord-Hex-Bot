using System;

namespace Hex_Game
{
    public class Position : IEquatable<Position>
    {
        public int x { get;  set; }
        public int y { get;   set; }

        public Position(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }

        public bool Equals(OrderedPair other)
        {
            return other.x == x && other.y == y;
        }

        #region Operators and Conversions
        // conversions
        // to and from (int, int) tuple
        public static implicit operator Position((int, int) t) => new Position(t.Item1, t.Item2);
        public static implicit operator (int, int)(Position p) => (p.x, p.y);

        // operators
        // Inverse/Negation
        public static OrderedPair operator -(OrderedPair a)
            => new OrderedPair(-a.x, -a.y);
        // Addition
        public static OrderedPair operator +(OrderedPair a, OrderedPair b)
            => new OrderedPair(a.x + b.x, a.y - b.y);
        // Subtraction
        public static OrderedPair operator -(OrderedPair a, OrderedPair b)
            => a + (-b);
        #endregion

        public override string ToString() => "({x}, {y})";

        public int distanceTo(Position position)
        {
            // Taxicab distance
            int distX = Math.Abs(position.x - this.x);
            int distY = Math.Abs(position.y - this.y);

            return distX + distY;
        }

    }

    public enum Direction
    {
        UP,
        DOWN,
        EAST,
        WEST
    }
}