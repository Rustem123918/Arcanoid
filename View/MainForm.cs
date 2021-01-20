using System;
using System.Drawing;
using System.Windows.Forms;

namespace Arcanoid
{
    sealed class MainForm : Form
    {
        private Game Game;
        public MainForm()
        {
            var label1 = new Label
            {
                Location = new Point(Game.PG_WIDTH+1, 0),
                Size = new Size(200, 20)
            };
            var label2 = new Label
            {
                Location = new Point(Game.PG_WIDTH+1, label1.Bottom),
                Size = label1.Size
            };
            var label3 = new Label
            {
                Location = new Point(Game.PG_WIDTH+1, label2.Bottom),
                Size = label1.Size
            };
            var label4 = new Label
            {
                Location = new Point(Game.PG_WIDTH + 1, label3.Bottom),
                Size = label1.Size
            };
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(label4);

            DoubleBuffered = true;
            ClientSize = new Size(Game.PG_WIDTH + Game.INFO_WIDTH, Game.PG_HEIGHT);

            Game = new Game(new Player(100, 20, new Point(0, 0)),
                            new Player(100, 20, new Point(0, Game.PG_HEIGHT - 20)),
                            new Ball(20,20, new Point(Game.PG_WIDTH/2, Game.PG_HEIGHT/2)));
            Game.StateChanged += (sender, args) =>
            {
                label1.Text = args.PlayerTop.Direction == Direction.Right ? "Right" : "Left";
                label2.Text = args.PlayerDown.Direction == Direction.Right ? "Right" : "Left";
                label3.Text = "Top player score: " + args.PlayerTop.Score.ToString();
                label4.Text = "Down player score: " + args.PlayerDown.Score.ToString();
            };

            Paint += MainForm_Paint;
            KeyDown += MainForm_KeyDown;

            var timer = new Timer();
            timer.Interval = Game.TIME_INTERVAL;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Left:
                    Game.PlayerDown.Direction = Direction.Left;
                    break;
                case Keys.Right:
                    Game.PlayerDown.Direction = Direction.Right;
                    break;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Game.MakeMove();
            Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawLine(new Pen(Color.Black), Game.PG_WIDTH, 0, Game.PG_WIDTH, Game.PG_HEIGHT);

            DrawPlayer(Game.PlayerTop, e);
            DrawPlayer(Game.PlayerDown, e);
            DrawBall(Game.Ball, e);
        }
        private void DrawPlayer(Player player, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(Brushes.Black, player.Position.X, player.Position.Y, player.Width, player.Height);
        }
        private void DrawBall(Ball ball, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(Brushes.Blue, ball.Position.X, ball.Position.Y, ball.Width, ball.Height);
        }
    }
}
