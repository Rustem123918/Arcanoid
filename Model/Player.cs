using System.Drawing;

namespace Arcanoid
{
    sealed class Player
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Score { get; set; }
        public Point Position { get; private set; }
        public Point CenterPosition { get => new Point(Position.X + Width / 2, Position.Y + Height / 2); }
        public int LeftBord { get => Position.X; }
        public int RightBord { get => Position.X + Width; }
        public Direction Direction { get; set; }
        public Player(int width, int height, Point startPosition)
        {
            Width = width;
            Height = height;
            Position = startPosition;
        }

        public void Move(int dx)
        {
            switch(Direction)
            {
                case Direction.Right:
                    Position = new Point(Position.X + dx, Position.Y);
                    break;
                case Direction.Left:
                    Position = new Point(Position.X - dx, Position.Y);
                    break;
            }
        }
    }
}
