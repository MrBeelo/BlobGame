using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Diagnostics;

namespace BlobGame
{
    public class Tilemap
    {
        public static int level = 0;
        public static int Tilesize = 32; //Display Tilesize
        public Point Mapsize {get; private set;}
        public static Dictionary<Vector2, int>[] Normal = new Dictionary<Vector2, int>[10 + 1]; //! Change based on how many maps you make.
        public static Dictionary<Vector2, int>[] Collision = new Dictionary<Vector2, int>[10 + 1]; //! Same here
        public Texture2D textureAtlas;
        public Texture2D hitboxAtlas;
        public static List<Vector3> normalTiles = new();
        public static List<Vector3> collisionTiles = new();

        public static List<Vector3> excludedNormalTiles = new();

        public static List<Vector3> excludedCollisionTiles = new();
        public static List<Vector3> permaExcludedNormalTiles = new();

        public static List<Vector3> permaExcludedCollisionTiles = new();

        public Tilemap()
        {
        }

        public Dictionary<Vector2, int> LoadMap(string filepath)
        {
            Dictionary<Vector2, int> result = new();

            StreamReader reader = new (filepath);
            int y = 0;
            string line;
            while((line = reader.ReadLine()) != null) {
                string[] Items = line.Split(',');
                for(int i = 0; i < Items.Length; i++)
                {
                    if(int.TryParse(Items[i], out int value)) {
                        if(value > -1) {
                            result[new Vector2(i, y)] = value;
                        }

                    }
                }
                y++;
            }
            return result;
        }

        public static void GetMapSize(string filepath, Tilemap tilemap)
        {
            StreamReader reader = new (filepath);
            int y = 0;
            string line;
            int x = 0; // Variable to store the number of columns
            while((line = reader.ReadLine()) != null) {
                string[] Items = line.Split(',');
                if (y == 0)
                {
                    x = Items.Length;
                }
                y++;
            }

            tilemap.Mapsize = new Point(x * Tilesize, y * Tilesize);
        }

        public void LoadContent(Game game)
        {
            string atlasPath = Path.Combine(AppContext.BaseDirectory, "data", "atlas.png");
            string collAtlasPath = Path.Combine(AppContext.BaseDirectory, "data", "collision_atlas.png");

            if (File.Exists(atlasPath))
            {
                using (var atlasTex = File.OpenRead(atlasPath))
                {
                    textureAtlas = Texture2D.FromStream(Globals.GraphicsDevice, atlasTex);
                }
            }
            else if (!File.Exists(atlasPath))
            {
                textureAtlas = game.Content.Load<Texture2D>("assets/atlas");
            }

            if (File.Exists(collAtlasPath))
            {
                using (var collAtlasTex = File.OpenRead(collAtlasPath))
                {
                    hitboxAtlas = Texture2D.FromStream(Globals.GraphicsDevice, collAtlasTex);
                }
            }
            else if (!File.Exists(collAtlasPath))
            {
                hitboxAtlas = game.Content.Load<Texture2D>("assets/collision_atlas");
            }

            for(int i = 0; i < Normal.Length; i++)
            {
                Normal[i] = LoadMap(Path.Combine(game.Content.RootDirectory, "..", "data", "level" + i + "_normal.csv"));
            }

            for(int i = 0; i < Collision.Length; i++)
            {
                Collision[i] = LoadMap(Path.Combine(game.Content.RootDirectory, "..", "data", "level" + i + "_collision.csv"));
            }
        }

        public void Update(Game game)
        {
            if(level > Collision.Length || level < 0)
            {
                level = 0;
            }

            //EvaluateLevelPos((int)level.X);
            
            GetMapSize(Path.Combine(game.Content.RootDirectory, "..", "data", "level" + Globals.SaveFile.Level + "_normal.csv"), this);
        }

        public void Draw(GameTime gameTime)
        {
            if(Main.currentGameState == Main.GameState.Playing)
            {
                int tpr = 8; //Tiles per row
                int p_tilesize = 32; //Pixel Tilesize

                foreach(var item in Normal[(int)Globals.SaveFile.Level])
                {
                    normalTiles.Add(new Vector3(item.Value, item.Key.X, item.Key.Y));
                    if(excludedNormalTiles.Contains(new Vector3(item.Value, item.Key.X, item.Key.Y))) continue;
                    if(permaExcludedNormalTiles.Contains(new Vector3(item.Value, item.Key.X, item.Key.Y))) continue;

                    Rectangle dest = new(
                        (int)item.Key.X * Tilesize,
                        (int)item.Key.Y * Tilesize,
                        Tilesize,
                        Tilesize
                    );

                    int x = item.Value % tpr;
                    int y = item.Value / tpr;

                    Rectangle src = new(
                        x * p_tilesize,
                        y * p_tilesize,
                        p_tilesize,
                        p_tilesize
                    );

                    Globals.SpriteBatch.Draw(textureAtlas, dest, src, Color.White);
                }

                foreach(var item in Collision[(int)Globals.SaveFile.Level])
                {
                    collisionTiles.Add(new Vector3(item.Value, item.Key.X, item.Key.Y));
                    if(excludedCollisionTiles.Contains(new Vector3(item.Value, item.Key.X, item.Key.Y))) continue;
                    if(permaExcludedCollisionTiles.Contains(new Vector3(item.Value, item.Key.X, item.Key.Y))) continue;

                    Rectangle dest = new(
                        (int)item.Key.X * Tilesize,
                        (int)item.Key.Y * Tilesize,
                        Tilesize,
                        Tilesize
                    );

                    int x = item.Value % tpr;
                    int y = item.Value / tpr;

                    Rectangle src = new(
                        x * p_tilesize,
                        y * p_tilesize,
                        p_tilesize,
                        p_tilesize
                    );

                    if(Main.hasF3On)
                    {
                        Globals.SpriteBatch.Draw(hitboxAtlas, dest, src, Color.White);
                    }
                }
            }
        }

        public static void MoveTo(int l)
        {
            //level.X++;
            level = l;
            //EvaluateLevel((int)level.X);
            //Player.Respawn(Main.player);
            Globals.SaveFile.SaveSavefile(Main.savefileFilePath);
            Eval();
            /*foreach (var tile in Collision[(int)level.X])
            {
                if (Collision[(int)level.X].TryGetValue(new Vector2(tile.Key.X, tile.Key.Y), out int value))
                {
                    //Debug.WriteLine("Level: " + level + ", Value: " + value + ", Pos: " + tile.Key.X +  ", " + tile.Key.Y);
                    if(value == 2)
                    {
                        Debug.WriteLine("PASS FOUND AT: " + new Vector2(tile.Key.X, tile.Key.Y));
                    }
                }
            }*/
        }

        public static void MoveLevel()
        {
            MoveTo((int)level + 1);
        }

        public static void Eval()
        {
            excludedNormalTiles.Clear();
            excludedCollisionTiles.Clear();
            Triangle.ClearAll();
            Circle.ClearAll();
            foreach (var tile in Collision[(int)level])
            {
                if (Collision[(int)level].TryGetValue(new Vector2(tile.Key.X, tile.Key.Y), out int value))
                {
                    if(value == 16)
                    {
                        Player.Teleport((int)tile.Key.X * Tilesize, (int)tile.Key.Y * Tilesize);
                    }

                    if(value == 17)
                    {
                        Triangle.Summon(new Vector2((int)tile.Key.X * Tilesize, (int)tile.Key.Y * Tilesize));
                    }

                    if(value == 18)
                    {
                        Triangle.SummonBoss(new Vector2((int)tile.Key.X * Tilesize, (int)tile.Key.Y * Tilesize));
                    }

                    if(value == 19)
                    {
                        Circle.Summon(new Vector2((int)tile.Key.X * Tilesize, (int)tile.Key.Y * Tilesize));
                    }
                }
            } 
        }

        public static void Reset(Player player)
        {
            level = 0;
            excludedNormalTiles.Clear();
            excludedCollisionTiles.Clear();
            Player.Respawn(player);
            Triangle.ClearAll();
            Circle.ClearAll();
        }

        /*public static void EvaluateLevelPos(int l)
        {
            switch(l)
            {
                case 0:
                    level.Y = 50;
                    level.Z = 600;
                    break;

                case 1:
                    level.Y = 100;
                    level.Z = 250;
                    break;

                case 2:
                    level.Y = 220;
                    level.Z = 190;
                    break;

                case 3:
                    level.Y = 200;
                    level.Z = 100;
                    break;

                case 4:
                    level.Y = 160;
                    level.Z = 65;
                    break;

                case 5:
                    level.Y = 100;
                    level.Z = 65;
                    break;

                case 6:
                    level.Y = 200;
                    level.Z = 100;
                    break;

                case 7:
                    level.Y = 225;
                    level.Z = 350;
                    break;
            }
        }

        public static void EvaluateLevel(int l)
        {
            EvaluateLevelPos(l);
            excludedNormalTiles.Clear();
            excludedCollisionTiles.Clear();
            Triangle.ClearAll();
            Circle.ClearAll();
            switch(l)
            {
                case 0:
                    break;

                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    Circle.Summon(new Vector2(1400, 400));
                    Circle.Summon(new Vector2(750, 250));
                    break;

                case 4:
                    Triangle.Summon(new Vector2(1600, 130));
                    Triangle.Summon(new Vector2(1090, 1000));
                    Triangle.Summon(new Vector2(580, 1025));
                    break;

                case 5:
                    Circle.Summon(new Vector2(800, 65));
                    Circle.Summon(new Vector2(1400, 130));
                    Triangle.Summon(new Vector2(1890, 130));
                    Circle.Summon(new Vector2(380, 320));
                    Circle.Summon(new Vector2(960, 320));
                    Triangle.Summon(new Vector2(1540, 320));
                    Circle.Summon(new Vector2(1800, 320));
                    Circle.Summon(new Vector2(290, 580));
                    Triangle.Summon(new Vector2(770, 580));
                    Circle.Summon(new Vector2(1570, 580));
                    Circle.Summon(new Vector2(670, 830));
                    Triangle.Summon(new Vector2(1180, 830));
                    Circle.Summon(new Vector2(1150, 1000));
                    Circle.Summon(new Vector2(1630, 1000));
                    break;

                case 6:
                    break;

                case 7:
                    break;
            }
        }*/
    }
}