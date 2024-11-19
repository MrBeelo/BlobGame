using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.PortableExecutable;

namespace BlobGame
{
    public class Settings
    {
        public float Volume { get; set; } = 0.5f; // Default volume
        public static Point SimulationSize = new Point(1920, 1080); // Default Resolution
        public Point WindowSize = new Point(1920, 1080);
        public void ChangeVolume(float delta)
        {
            Volume = MathHelper.Clamp(Volume + delta, 0f, 1.0f); // Clamp between 0 and 1
        }

        public void SetResolution(int height, int width)
        {
            Main.main.Window.IsBorderless = false;
            Main.main.IsMouseVisible = true;
            WindowSize.X = height;
            WindowSize.Y = width;
            Globals.Graphics.ApplyChanges();
            Main.main.canvas.SetDestinationRectangle();
        }

        public void SetFullScreen()
        {
            if(!Main.main.Window.IsBorderless)
            {
                SetResolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
                Main.main.Window.IsBorderless = true;
                Main.main.IsMouseVisible = false;
                Globals.Graphics.ApplyChanges(); 
            } else if(Main.main.Window.IsBorderless) {
                SetResolution(1920, 1080);
            }
            Main.main.canvas.SetDestinationRectangle();  
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