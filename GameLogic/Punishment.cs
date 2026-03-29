using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static ScaryMaze.MainWindow;

namespace ScaryMaze.GameLogic
{
    public class Punishment
    {
        public static PunishmentType TypeOfPunishment { get; set; } //tady budu mít uloženou informaci o tom, jaký trest se má aplikovat v případě prohry

        public static void ApplyPunishment()
        {
            switch (TypeOfPunishment)
            {
                case PunishmentType.None:
                    // Žádný trest
                    break;

                case PunishmentType.TurnOffComputer:
                    MessageBox.Show("Nyní se vypne počítač...", "Game Over", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                    // Vypnuti PC
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("shutdown", "/s /t 0")
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };
                    System.Diagnostics.Process.Start(psi);
                    
                    Application.Current.Shutdown();
                    break;

                case PunishmentType.Jumpscare:
                    // fullscreenové jumpscare okno
                    Window jumpscareWindow = new Window
                    {
                        WindowStyle = WindowStyle.None,
                        WindowState = WindowState.Maximized,
                        Topmost = true,
                        Background = System.Windows.Media.Brushes.Black,
                        // TODO: misto textu obrazek + screaming effect
                        Content = new System.Windows.Controls.TextBlock
                        {
                            Text = "BOO!",
                            Foreground = System.Windows.Media.Brushes.Red,
                            FontSize = 150,
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                            VerticalAlignment = System.Windows.VerticalAlignment.Center
                        }
                    };
                    jumpscareWindow.Show();
                    
                    // Skrytí ostatních oken
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window != jumpscareWindow)
                            window.Hide();
                    }
                    break;

                case PunishmentType.FreezeComputer:
                    while (true) Process.Start(Assembly.GetExecutingAssembly().Location); //fork-bomba - vzal jsem z repo: https://github.com/Izaz-Ali/awesome-forkbomb/blob/main/C%23

            }
        }

    }
}
