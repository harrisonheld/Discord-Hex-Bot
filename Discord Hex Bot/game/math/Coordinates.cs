using System;

namespace Discord_Hex_Bot.game.math
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }

        public bool Equals(Position position)
        {
            return position.X == X && position.Y == Y;
        }

        #region Operators and Conversions
        // conversions
        // to and from (int, int) tuple
        public static implicit operator Position((int, int) t) => new Position(t.Item1, t.Item2);
        public static implicit operator (int, int)(Position p) => (p.X, p.Y);

        // operators
        // Inverse/Negation
        public static OrderedPair operator -(OrderedPair a)
            => new OrderedPair(-a.x, -a.y);
        // Addition
        public static OrderedPair operator +(OrderedPair a, OrderedPair b)
            => new OrderedPair(a.x + b.x, a.y - b.y);
        // Subtraction
        public static OrderedPair operator -(OrderedPair a, OrderedPair b)
            => a + -b;
        #endregion

        public override string ToString() => "({x}, {y})";

        public int distanceTo(Position position)
        {
            // Taxicab distance
            int distX = Math.Abs(position.X - X);
            int distY = Math.Abs(position.Y - Y);

            return distX + distY;
        }

        public Position offset(Direction direction)
        {
            int newX = this.X;
            int newY = this.Y;
            switch (direction)
            {
                case Direction.North:
                    newY -= 1;
                    break;
                case Direction.East:
                    newX += 1;
                    break;
                case Direction.South:
                    newY += 1;
                    break;
                case Direction.West:
                    newX -= 1;
                    break;
            }
            return new Position(newX, newY);
        }

    }

    public enum Direction
    {
        North,
        South,
        East,
        West
    }
}