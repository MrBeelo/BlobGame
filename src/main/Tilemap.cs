using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace BlobGame
{
    public class Tilemap
    {
        public static int level = 0;
        public static int Tilesize = 32; //Display Tilesize
        public System.Drawing.Point Mapsize {get; private set;}
        public static Dictionary<Vector2, int>[] Normal = new Dictionary<Vector2, int>[10 + 1]; //! Change based on how many maps you make.
        public static Dictionary<Vector2, int>[] Collision = new Dictionary<Vector2, int>[10 + 1]; //! Same here
        public Texture2D textureAtlas;
        public Texture2D hitboxAtlas;
        public static List<Rectangle> spikes = new();
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

            tilemap.Mapsize = new System.Drawing.Point(x * Tilesize, y * Tilesize);
        }

        public void LoadContent(Game game)
        {
            textureAtlas = LoadTexture("assets/atlases/atlas.png");
            hitboxAtlas = LoadTexture("assets/atlases/collision_atlas.png");

            for(int i = 0; i < Normal.Length; i++)
            {
                Normal[i] = LoadMap("data/level" + i + "_normal.csv");
            }

            for(int i = 0; i < Collision.Length; i++)
            {
                Collision[i] = LoadMap("data/level" + i + "_collision.csv");
            }
        }

        public void Update(Game game)
        {
            if(level > Collision.Length || level < 0)
            {
                level = 0;
            }
            
            GetMapSize("data/level" + Globals.SaveFile.Level + "_collision.csv", this);
        }

        public void Draw()
        {
            if(Game.currentGameState == Game.GameState.Playing)
            {
                int tpr = 8; //Tiles per row
                int p_tilesize = 32; //Pixel Tilesize

                foreach(var item in Normal[Globals.SaveFile.Level])
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

                    DrawTexturePro(textureAtlas, src, dest, Vector2.Zero, 0f, Color.White);
                }

                foreach(var item in Collision[Globals.SaveFile.Level])
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

                    if(Game.hasF3On)
                    {
                        DrawTexturePro(hitboxAtlas, src, dest, Vector2.Zero, 0f, Color.White);
                    }
                }
            }
        }

        public static void MoveTo(int l)
        {
            level = l;
            Globals.SaveFile.SaveSavefile(Game.savefileFilePath);
            Eval();
        }

        public static void MoveLevel()
        {
            MoveTo(level + 1);
        }

        public static void Eval()
        {
            spikes.Clear();
            excludedNormalTiles.Clear();
            excludedCollisionTiles.Clear();
            Triangle.ClearAll();
            TriangleBoss.ClearAll();
            Circle.ClearAll();
            foreach (var tile in Collision[level])
            {
                if (Collision[level].TryGetValue(new Vector2(tile.Key.X, tile.Key.Y), out int value))
                {
                    if(value == 11)
                    {
                        Rectangle spike = new ((int)tile.Key.X * Tilesize + 8, (int)tile.Key.Y * Tilesize + 12, 16, 20);
                        spikes.Add(spike);
                    }

                    if(value == 12)
                    {
                        Rectangle spike = new ((int)tile.Key.X * Tilesize, (int)tile.Key.Y * Tilesize + 8, 20, 16);
                        spikes.Add(spike);
                    }

                    if(value == 13)
                    {
                        Rectangle spike = new ((int)tile.Key.X * Tilesize + 8, (int)tile.Key.Y * Tilesize, 16, 20);
                        spikes.Add(spike);
                    }

                    if(value == 14)
                    {
                        Rectangle spike = new ((int)tile.Key.X * Tilesize + 12, (int)tile.Key.Y * Tilesize + 8, 20, 16);
                        spikes.Add(spike);
                    }

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
                        TriangleBoss.Summon(new Vector2((int)tile.Key.X * Tilesize, (int)tile.Key.Y * Tilesize));
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
            TriangleBoss.ClearAll();
            Circle.ClearAll();
        }
    }
}