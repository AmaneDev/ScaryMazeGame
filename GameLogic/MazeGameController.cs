using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace ScaryMaze.GameLogic
{
    public class MazeGameController
    {
        private Window levelWindow;

        public MazeGameController(Window window)
        {
            levelWindow = window;
        }

        public static int CurrentLevel { get; internal set; }

        public void CheckCollision(Player player, List<Rectangle> rectangles)
        {
            bool onSafe = false;

            foreach (var area in rectangles)
            {
                if (area == null) continue;

                Rect playerRect = new Rect(Canvas.GetLeft(player.UI), Canvas.GetTop(player.UI),
                                           player.UI.Width, player.UI.Height);

                Rect areaRect = new Rect(Canvas.GetLeft(area), Canvas.GetTop(area),
                                         area.Width, area.Height);

                if (!playerRect.IntersectsWith(areaRect)) continue;

                string tag = area.Tag as string;

                if (tag == "Win")
                {
                    var nextLevel = new Level(CurrentLevel + 1); //vytvorim si objekt noveho levelu v parametru nactu dalsi level (o lvl vyssi nez stavajici) a posleze zavolam metodu pro samotne otevreni okna
                    nextLevel.OpenLevelWindow();
                    levelWindow.Close();
                    return;
                }

                if (tag == "Safe")
                {
                    onSafe = true;
                }
            }

            // Pokud hráč není na žádném Safe, prohra
            if (!onSafe)
                Lose();
        }

        private void Lose()
        {
            //MessageBox.Show("Prohrál jsi!");
            Punishment.ApplyPunishment();
            levelWindow.Close();
        }
    }
}
