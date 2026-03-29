using System;
using System.Reflection;
using System.Windows;

namespace ScaryMaze.GameLogic
{
    public class Level
    {
        public int LevelId { get; private set; }

        public Level(int levelId)
        {
            LevelId = levelId;
        }

        public void OpenLevelWindow()
        {
            string className = $"Level{LevelId}";
            string fullName = $"ScaryMaze.LevelWindows.{className}";    //dynamicky si podle namespace vybuildim nazev tridy

            Type type = Assembly.GetExecutingAssembly().GetType(fullName);

            if (type == null)
                throw new Exception($"Level window {fullName} not found.");

            Window levelWindow = (Window)Activator.CreateInstance(type);  //zde si vytvorim instanci tridy
            levelWindow.ShowDialog();
        }
    }
}