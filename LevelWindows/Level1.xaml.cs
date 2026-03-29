
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using ScaryMaze.GameLogic;

namespace ScaryMaze.LevelWindows
{
    public partial class Level1 : Window
    {
        private Player player;
        private MazeGameController controller;
        private Point mouseOffset;

        public Level1()
        {
            InitializeComponent();

            Cursor = Cursors.None;

            player = new Player(PlayerRect);
            controller = new MazeGameController(this);

            SetPositionToStart();

            // startovní offset podle aktuální pozice myši
            GameCanvas.Loaded += (s, e) =>
            {
                Point pos = Mouse.GetPosition(GameCanvas);
                mouseOffset = new Point(pos.X - Canvas.GetLeft(PlayerRect) - PlayerRect.Width / 2,
                                        pos.Y - Canvas.GetTop(PlayerRect) - PlayerRect.Height / 2);
            };
        }
        public void SetPositionToStart()
        {
            double startX = Canvas.GetLeft(PlayerRect) + PlayerRect.Width / 2;
            double startY = Canvas.GetTop(PlayerRect) + PlayerRect.Height / 2;
            player.Move(startX, startY);
        }

        private void CaptureMouseStart(object sender, MouseEventArgs e)
        {
            // Startovní offset – kolikrát je hráč od kurzoru
            Point pos = e.GetPosition(GameCanvas);
            mouseOffset = new Point(pos.X - Canvas.GetLeft(PlayerRect) - PlayerRect.Width / 2,
                                    pos.Y - Canvas.GetTop(PlayerRect) - PlayerRect.Height / 2);

            // od této chvíle přepneme na normální GameCanvas_MouseMove
            this.MouseMove -= CaptureMouseStart;
            GameCanvas.MouseMove += GameCanvas_MouseMove;
        }

        private void GameCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(GameCanvas);
            player.Move(pos.X - mouseOffset.X, pos.Y - mouseOffset.Y);

            var rectangles = GameCanvas.Children
                .OfType<Rectangle>()
                .Where(r => r != PlayerRect) // vynecháme hráče
                .ToList();

            controller.CheckCollision(player, rectangles);
        }


    }
}
