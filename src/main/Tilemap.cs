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
        public static int Tilesize = 30; //Display Tilesize
        public static Dictionary<Vector2, int>[] Normal;
        public static Dictionary<Vector2, int>[] Collision;
        public Texture2D textureAtlas;
        public Texture2D hitboxAtlas;

        public Tilemap()
        {
            Normal = new Dictionary<Vector2, int>[1];
            Collision = new Dictionary<Vector2, int>[1];
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
            Normal[0] =LoadMap(Path.Combine(game.Content.RootDirectory, "..", "data", "testlevel_normal.csv"));
            Collision[0] = LoadMap(Path.Combine(game.Content.RootDirectory, "..", "data", "testlevel_collision.csv"));

            textureAtlas = game.Content.Load<Texture2D>("assets/atlas");
            hitboxAtlas = game.Content.Load<Texture2D>("assets/collision_atlas");
        }

        public void Draw(GameTime gameTime)
        {
            if(Main.currentGameState == Main.GameState.Playing)
            {
                int tpr = 8; //Tiles per row
                int p_tilesize = 16; //Pixel Tilesize
                foreach(var item in Normal[(int)level.X])
                {
                    if(Main.player.stamina < 500)
                    {
                        if(item.Value == 14) continue;
                    }

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
                    if(Main.player.stamina < 500)
                    {
                        if(item.Value == 5) continue;
                    }

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