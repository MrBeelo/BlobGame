using System.Numerics;
using Newtonsoft.Json;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace BlobGame
{
    public class SaveFile
    {
        public int Level {get; set;}
        public int Xartomantila {get; set;}
        public List<Vector3> permaExcludedNormalTiles {get; set;}

        public List<Vector3> permaExcludedCollisionTiles {get; set;}

        public void Initialize()
        {
            LoadAndApplySaveFile(Game.savefileFilePath);
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
                Tilemap.Eval();
            } else if(!File.Exists(filePath))
            {
                Level = 0;
                Xartomantila = 0;
                permaExcludedNormalTiles = new();
                permaExcludedCollisionTiles = new();
                SaveSavefile(Game.savefileFilePath);
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