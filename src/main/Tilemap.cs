using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace BlobGame
{
    public class Tilemap
    {
        public static Vector3 level = new Vector3(0, 50, 600);
        public static int Tilesize = 32; //Display Tilesize
        public static Dictionary<Vector2, int>[] Normal;
        public static Dictionary<Vector2, int>[] Collision;
        public Texture2D textureAtlas;
        public Texture2D hitboxAtlas;
        public List<Vector3> normalTiles;
        public List<Vector3> collisionTiles;

        public static List<Vector3> excludedNormalTiles;

        public static List<Vector3> excludedCollisionTiles;

        public Tilemap()
        {
            Normal = new Dictionary<Vector2, int>[3]; //! Change based on how many maps you make.
            Collision = new Dictionary<Vector2, int>[3]; //! Same here
            normalTiles = new();
            collisionTiles = new();
            excludedNormalTiles = new();
            excludedCollisionTiles = new();
        }

        public Dictionary<Vector2, int> LoadMap(string filepath)
        {
            Dictionary<Vector2, int> result = new();

            StreamReader reader = new (filepath);
            int y = 0;
            string line;
            while((line = reader.ReadLine()) != null) {
                string[] Items = line.Split(',');
                for(int x = 0; x < Items.Length; x++)
                {
                    if(int.TryParse(Items[x], out int value)) {
                        if(value > -1) {
                            result[new Vector2(x, y)] = value;
                        }

                    }
                }

                y++;
            }

            return result;

        }

        public void LoadContent(Game game)
        {
            textureAtlas = game.Content.Load<Texture2D>("assets/atlas");
            hitboxAtlas = game.Content.Load<Texture2D>("assets/collision_atlas");

            Normal[0] = LoadMap(Path.Combine(game.Content.RootDirectory, "..", "data", "level0_normal.csv"));
            Normal[1] = LoadMap(Path.Combine(game.Content.RootDirectory, "..", "data", "level1_normal.csv"));
            Normal[2] = LoadMap(Path.Combine(game.Content.RootDirectory, "..", "data", "level2_normal.csv"));
            //! Import all level CSVs for the normal tileset here

            Collision[0] = LoadMap(Path.Combine(game.Content.RootDirectory, "..", "data", "level0_collision.csv"));
            Collision[1] = LoadMap(Path.Combine(game.Content.RootDirectory, "..", "data", "level1_collision.csv"));
            Collision[2] = LoadMap(Path.Combine(game.Content.RootDirectory, "..", "data", "level2_collision.csv"));
            //! Import all level CSVs for the collision tileset here
        }

        public void Draw(GameTime gameTime)
        {
            if(Main.currentGameState == Main.GameState.Playing)
            {
                int tpr = 8; //Tiles per row
                int p_tilesize = 32; //Pixel Tilesize

                foreach(var item in Normal[(int)level.X])
                {
                    normalTiles.Add(new Vector3(item.Value, item.Key.X, item.Key.Y));
                    if(excludedNormalTiles.Contains(new Vector3(item.Value, item.Key.X, item.Key.Y))) continue;

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

                foreach(var item in Collision[(int)level.X])
                {
                    collisionTiles.Add(new Vector3(item.Value, item.Key.X, item.Key.Y));
                    if(excludedCollisionTiles.Contains(new Vector3(item.Value, item.Key.X, item.Key.Y))) continue;

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
                        //DrawRectHollow(Globals.SpriteBatch, dest, 2, Color.Orange);
                    }
                }
            }
        }
    }
}