using System;
using System.Drawing;

namespace Arcanoid
{
    internal class StateEventArgs : EventArgs
    {
        public Player PlayerTop { get; set; }
        public Player PlayerDown { get; set; }
        public Ball Ball { get; set; }
    }
    internal delegate void StateEventHandler(object sender, StateEventArgs args);
    sealed class Game
    {
        public const int PG_WIDTH = 600;
        public const int PG_HEIGHT = 600;
        public const int INFO_WIDTH = 400;
        public const int TIME_INTERVAL = 15;
        public const int DELTA_X = 5;

        public event StateEventHandler StateChanged;
        public Player PlayerTop { get; private set; }
        public Player PlayerDown { get; private set; }
        public Ball Ball { get; private set; }

        public Game(Player playerTop, Player playerDown, Ball ball)
        {
            PlayerTop = playerTop;
            PlayerDown = playerDown;
            Ball = ball;
        }
        public void MakeMove()
        {
            if (PlayerTop.CenterPosition.X > Ball.CenterPosition.X)
                PlayerTop.Direction = Direction.Left;
            else
                PlayerTop.Direction = Direction.Right;
            MovePlayer(PlayerTop);
            MovePlayer(PlayerDown);
            MoveBall();
            StateChanged?.Invoke(this, new StateEventArgs { PlayerTop = PlayerTop, PlayerDown = PlayerDown, Ball = Ball });
        }
        private void MovePlayer(Player player)
        {
            player.Move(DELTA_X);
            if (player.RightBord >= PG_WIDTH)
                player.Direction = Direction.Left;
            else if (player.LeftBord <= 0)
                player.Direction = Direction.Right;
        }
        private void MoveBall()
        {
            Ball.Move(DELTA_X);
            if(Ball.RightBord >= PG_WIDTH)
            {
                var alpha = Ball.DirectionVector.Angle;
                var alphaSign = Math.Sign(alpha);
                alpha = Math.Abs(alpha);
                var betta = Math.PI / 2 - alpha;
                var nextAlpha = 2 * betta;
                nextAlpha *= alphaSign;
                Ball.DirectionVector = Ball.DirectionVector.Rotate(nextAlpha);
                //Ставим шарик на границу по оси X, чтобы не было зацикливания
                Ball.CenterPosition = new Point(PG_WIDTH - Ball.Width / 2, Ball.CenterPosition.Y);
            }
            else if(Ball.LeftBord <= 0)
            {
                var alpha = Ball.DirectionVector.Angle;
                var alphaSign = Math.Sign(alpha);
                alpha = Math.Abs(alpha);
                var betta = alpha - Math.PI / 2;
                var nextAlpha = 2 * betta;
                nextAlpha *= alphaSign;
                Ball.DirectionVector = Ball.DirectionVector.Rotate(-nextAlpha);
                //Ставим шарик на границу по оси X, чтобы не было зацикливания
                Ball.CenterPosition = new Point(Ball.Width / 2 + 1, Ball.CenterPosition.Y);
            }
            if(Ball.TopBord <= PlayerTop.Height && 
                Ball.CenterPosition.X >= PlayerTop.LeftBord && 
                Ball.CenterPosition.X <= PlayerTop.RightBord)
            {
                var alpha = Ball.DirectionVector.Angle;
                var nextAlpha = -2 * alpha;
                Ball.DirectionVector = Ball.DirectionVector.Rotate(nextAlpha);
                
            }
            else if(Ball.DownBord >= PlayerDown.Position.Y &&
                Ball.CenterPosition.X >= PlayerDown.LeftBord &&
                Ball.CenterPosition.X <= PlayerDown.RightBord)
            {
                var alpha = Ball.DirectionVector.Angle;
                var nextAlpha = -2 * alpha;
                Ball.DirectionVector = Ball.DirectionVector.Rotate(nextAlpha);
            }
            if(Ball.TopBord <= 0)
            {
                ScoreAndBallRespawn(PlayerDown);
            }
            else if(Ball.DownBord >= PG_HEIGHT)
            {
                ScoreAndBallRespawn(PlayerTop);
            }

        }
        private void ScoreAndBallRespawn(Player player)
        {
            player.Score++;
            Ball.CenterPosition = new Point(PG_WIDTH / 2, PG_HEIGHT / 2);
            Ball.DirectionVector = Vector.GenerateRandomVector();
        }
    }
}
