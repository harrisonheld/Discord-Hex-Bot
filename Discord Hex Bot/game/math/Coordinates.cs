using System;

namespace Discord_Hex_Bot.game.math
{
    public class Position :IComparable

    {
        public int getValue()
        {
            return Settings.MAP_WIDTH * this.Y + this.X;
        }
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
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
        public static Position operator -(Position a)
            => new Position(-a.X, -a.Y);
        // Addition
        public static Position operator +(Position a, Position b)
            => new Position(a.X + b.X, a.Y - b.Y);
        // Subtraction
        public static Position operator -(Position a, Position b)
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

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Position other = obj as Position;
            if (other != null)
            {
                return this.getValue().CompareTo(other.getValue());
            }
            else
                throw new ArgumentException("Object is not a Position");
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