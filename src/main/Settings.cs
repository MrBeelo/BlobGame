using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace BlobGame
{
    public class Settings
    {
        public float Volume { get; set; } = 0.5f; // Default volume
        public void ChangeVolume(float delta)
        {
            Volume = MathHelper.Clamp(Volume + delta, 0f, 1.0f); // Clamp between 0 and 1
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