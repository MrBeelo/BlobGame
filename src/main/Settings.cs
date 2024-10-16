using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace BlobGame
{
    public class Settings
    {
        public float Volume { get; set; } = 0.5f; // Default volume

        //public Vector2 PlayerPos {get; set;} = new Vector2(50, 600);
        //public float Level {get; set;}

        //public Resolution Resolution { get; set; } = new Resolution { Width = 1920, Height = 1080 }; // Default resolution

        public void ChangeVolume(float delta)
        {
            Volume = MathHelper.Clamp(Volume + delta, 0f, 1.0f); // Clamp between 0 and 1
        }

        /*public void ChangeResolution(int width, int height)
        {
            Resolution.Width = width;
            Resolution.Height = height;
        }*/

        /*public Vector2 GetPlayerPos(string PlayerPos)
        {
            var posValues = PlayerPos.Split(',');
            return new Vector2(float.Parse(posValues[0]), float.Parse(posValues[1]));
        }*/

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