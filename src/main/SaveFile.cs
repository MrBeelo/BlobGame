using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlobGame
{
    public class SaveFile
    {
        public Vector3 Level {get; set;}
        public int Xartomantila {get; set;}
        public List<Vector3> permaExcludedNormalTiles {get; set;}

        public List<Vector3> permaExcludedCollisionTiles {get; set;}

        public void Initialize()
        {
            LoadAndApplySaveFile(Main.savefileFilePath);
            Tilemap.level = Level;
            Player.xartomantila = Xartomantila;
            Tilemap.permaExcludedNormalTiles = permaExcludedNormalTiles;
            Tilemap.permaExcludedCollisionTiles = permaExcludedCollisionTiles;
            Update();
        }

        public void Update()
        {
            Level = Tilemap.level;
            Xartomantila = Player.xartomantila;
            permaExcludedNormalTiles = Tilemap.permaExcludedNormalTiles;
            permaExcludedCollisionTiles = Tilemap.permaExcludedCollisionTiles;
        }

        public static SaveFile LoadSavefile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<SaveFile>(json);
            }
            return new SaveFile(); // Return default settings if file does not exist
        }

        public void LoadAndApplySaveFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var loadedSaveFile = JsonConvert.DeserializeObject<SaveFile>(json);

                // Update static properties
                Level = loadedSaveFile.Level;
                Xartomantila = loadedSaveFile.Xartomantila;
                permaExcludedNormalTiles = loadedSaveFile.permaExcludedNormalTiles;
                permaExcludedCollisionTiles = loadedSaveFile.permaExcludedCollisionTiles;
            } else if(!File.Exists(filePath))
            {
                Level = new Vector3(0, 50, 600);
                Xartomantila = 0;
                permaExcludedNormalTiles = new();
                permaExcludedCollisionTiles = new();
                SaveSavefile(Main.savefileFilePath);
            }
        }

        public void SaveSavefile(string filePath)
        {
            Update();
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}