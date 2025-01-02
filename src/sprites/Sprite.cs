using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace BlobGame
{
    public class Sprite
    {
        public Texture2D Texture;
        public Rectangle Drect;
        public Rectangle Srect;
        public Vector2 Position;
        public Vector2 TilePosition;

        public Sprite(Texture2D texture, Rectangle drect, Rectangle srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Position = new Vector2(drect.X, drect.Y);
            TilePosition = new Vector2(drect.X / 32, drect.Y / 32);
        }
        public virtual void LoadContent(Game game) {}
        public virtual void Update() {
            Position = new Vector2(Drect.X, Drect.Y);
            TilePosition = new Vector2(Drect.X / 32, Drect.Y / 32);
        }

        public virtual void Draw() {}
        public virtual string[] GetDebugInfo() {
            return new string[] {};
        }
    }
}