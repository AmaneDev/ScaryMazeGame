using ScaryMaze.GameLogic;
using ScaryMaze.LevelWindows;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScaryMaze
{
    public partial class MainWindow : Window
    {
        string CSVLevelData = "levelData.csv";
        MediaPlayer bgMenuMusic = new MediaPlayer();
        int CountOfLevels = 10; //počet levelů

        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists(CSVLevelData))
                CreateDefaultCsv();

            LoadMusic();
            LoadVideo();
            LoadLevels();
        }

        public enum LevelStatus
        {
            NotPlayed,
            RecentlyPlayedFailed,
            RecentlyPlayedDone
        }

        public enum PunishmentType
        {
            TurnOffComputer,
            Jumpscare,
            FreezeComputer,
            None
        }


        private void LoadMusic()
        {
            string relativePath = "Assets/bgMenuMusic.mp3";
            string absolutePath = Path.GetFullPath(relativePath);

            bgMenuMusic.Open(new Uri(absolutePath));
            bgMenuMusic.Volume = 0.5;
            bgMenuMusic.Play();
        }

        private void LoadVideo()
        {
            string relativePath = "Assets/Glitch.mp4";
            string absolutePath = Path.GetFullPath(relativePath);

            BackgroundVideo.Source = new Uri(absolutePath);
            BackgroundVideo.Play();
        }

        private void BackgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundVideo.Position = TimeSpan.Zero;
            BackgroundVideo.Play();
        }


        private void LoadLevels() //načte levely z CSV a vytvoří pro ně tlačítka, které se podle stavu levelu zbarví a po kliknutí spustí daný level
        {
            // Vyčištění starých tlačítek z WrapPanelu (paklize resetuju levely pres button Reset levels)
            LevelPanel.Children.Clear();
            var lines = File.ReadAllLines(CSVLevelData);

            foreach (string line in lines)
            {
                var CSVDataLineArray = line.Split(';');

                int levelId = int.Parse(CSVDataLineArray[0]);
                LevelStatus levelStatus =
                    Enum.Parse<LevelStatus>(CSVDataLineArray[1]);

                Button levelButton = new Button
                {
                    Width = 80,
                    Height = 80,
                    Margin = new Thickness(10),
                    Content = levelId.ToString(),
                    FontSize = 20
                };

                switch (levelStatus)
                {
                    case LevelStatus.NotPlayed:
                        levelButton.Background = Brushes.Gray;
                        break;

                    case LevelStatus.RecentlyPlayedFailed:
                        levelButton.Background = Brushes.DarkRed;
                        break;

                    case LevelStatus.RecentlyPlayedDone:
                        levelButton.Background = Brushes.DarkGreen;
                        break;
                }

                int capturedLevel = levelId;

                levelButton.Click += (s, e) => StartLevel(capturedLevel);

                LevelPanel.Children.Add(levelButton);
            }
        }

        private void StartLevel(int levelId)
        {
            if(GetSelectedPunishment() == PunishmentType.None) /*kontroluju, jestli si hrac vybral trest*/
            {
                MessageBox.Show("Please select a punishment before starting the level.", "No Punishment Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                GameLogic.Punishment.TypeOfPunishment = GetSelectedPunishment(); //nastavím trest, kterej se bude aplikovat v případě prohry jako atribut objektu Punishment, kterej je statickej, takže se k němu můžu dostat odkudkoliv a nemusím ho předávat mezi oknama
            }
            this.Hide(); //skryju si behem otevreneho levelu mainWindow
            MazeGameController.CurrentLevel = levelId;
            var levelWindow = new Level(levelId);
            levelWindow.OpenLevelWindow();

        }

        private void CreateDefaultCsv()   //vytvoří defaultní game data se zaznami o pokroku v levelech
        {
            var levels = new List<string>();

            for (int i = 1; i <= CountOfLevels; i++)
            {
                levels.Add($"{i};NotPlayed");
            }

            File.WriteAllLines(CSVLevelData, levels);
        }
        public PunishmentType GetSelectedPunishment()
        {
            if (rbTurnOff.IsChecked == true)
                return PunishmentType.TurnOffComputer;

            if (rbJumpscare.IsChecked == true)
                return PunishmentType.Jumpscare;

            if (rbFreeze.IsChecked == true)
                return PunishmentType.FreezeComputer;

            return PunishmentType.None; //žadnej radio button není vybraný
        }

        private void btnExitGame_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   
        }

        private void btnResetLevels_Click(object sender, RoutedEventArgs e)
        {
            File.Delete(CSVLevelData);
            CreateDefaultCsv();
            LoadLevels();
        }
    }
}