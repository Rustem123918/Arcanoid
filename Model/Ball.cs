using System;
using System.Drawing;

namespace Arcanoid
{
    sealed class Ball
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Point Position { get => new Point(CenterPosition.X - Width / 2, CenterPosition.Y - Height / 2); }
        public Point CenterPosition { get; set; }
        public int LeftBord { get => CenterPosition.X - Width/2; }
        public int RightBord { get => CenterPosition.X + Width/2; }
        public int TopBord { get => CenterPosition.Y - Height/2; }
        public int DownBord { get => CenterPosition.Y + Height/2; }
        public Vector DirectionVector { get; set; }

        public Ball(int width, int height, Point centerPosition)
        {
            Width = width;
            Height = height;
            CenterPosition = centerPosition;
            DirectionVector = Vector.GenerateRandomVector();
        }
        public void Move(int delta)
        {
            var x = CenterPosition.X;
            var y = CenterPosition.Y;
            var nextX = (int)(x + delta * Math.Cos(DirectionVector.Angle));
            var nextY = (int)(y + delta * Math.Sin(DirectionVector.Angle));
            CenterPosition = new Point(nextX, nextY);
        }
    }
}
