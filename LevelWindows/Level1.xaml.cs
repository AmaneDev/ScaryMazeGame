
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
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace ScaryMaze.LevelWindows
{
    public partial class Level1 : Window
    {
        private Player player;
        private MazeGameController controller;
        private Point mouseOffset;
        private bool isLevelStarted;

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        public Level1()
        {
            InitializeComponent();

            Cursor = Cursors.None;

            player = new Player(PlayerRect);
            controller = new MazeGameController(this);
            isLevelStarted = false;

            SetPositionToStart();
            Loaded += (_, __) => InitializeLevelStart();
        }
        public void SetPositionToStart()
        {
            double startX = Canvas.GetLeft(PlayerRect) + PlayerRect.Width / 2;
            double startY = Canvas.GetTop(PlayerRect) + PlayerRect.Height / 2;
            player.Move(startX, startY);
        }

        private void ForceCursorToPlayerStart()
        {
            Point playerPoint = PlayerRect.TransformToAncestor(this)
                .Transform(new Point(PlayerRect.Width / 2, PlayerRect.Height / 2));

            Point screenPoint = PointToScreen(playerPoint);
            SetCursorPos((int)screenPoint.X, (int)screenPoint.Y);
        }

        private void InitializeLevelStart()
        {
            SetPositionToStart();

            // posune myš na startovní pozici hráče až po načtení okna, aby se zabránilo problémům s nastavením kurzoru před zobrazením okna
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SetPositionToStart();
                ForceCursorToPlayerStart();
                mouseOffset = new Point(0, 0);
                isLevelStarted = true;
            }), DispatcherPriority.ContextIdle);
        }

        private void GameCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isLevelStarted)
                return;

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
