using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Newtonsoft.Json;

namespace BlobGame
{
    public class Settings
    {
        public float Volume { get; set; } = 0.5f; // Default volume
        public static System.Drawing.Point SimulationSize = new System.Drawing.Point(1920, 1080); // Default Resolution
        public System.Drawing.Point WindowSize = new System.Drawing.Point(1920, 1080);
        public void ChangeVolume(float delta)
        {
            Volume = Math.Clamp(Volume + delta, 0f, 1.0f); // Clamp between 0 and 1
        }

        public void SetResolution(int height, int width)
        {
            if(IsWindowFullscreen())
            {
                ToggleFullscreen();
            }
            if(IsCursorHidden())
            {
                ShowCursor();
            }
            WindowSize.X = height;
            WindowSize.Y = width;
            Game.game.canvas.SetDestinationRectangle();
        }
        public static Settings LoadSettings(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Settings>(json);
            }
            return new Settings(); // Return default settings if file does not exist
        }

        public void SaveSettings(string filePath)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}