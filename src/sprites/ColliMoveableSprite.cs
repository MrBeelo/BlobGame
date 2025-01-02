using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace BlobGame
{
    public class CollMoveableSprite : MoveableSprite
    {
        public List<System.Drawing.Point> horizontalCollisions = new();
        public List<System.Drawing.Point> verticalCollisions = new();
        public Vector2 minPos, maxPos;
        public CollMoveableSprite(Texture2D texture, Rectangle drect, Rectangle srect) : base(texture, drect, srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Velocity = new();
        }

        public virtual void LoadContent()
        {
            horizontalCollisions = GetIntersectingTiles(Drect);
            verticalCollisions = GetIntersectingTiles(Drect);
        }

        public List<System.Drawing.Point> GetIntersectingTiles(Rectangle target)
        {
            List<System.Drawing.Point> tiles = new List<System.Drawing.Point>();
        
            int leftTile = (int)(target.Left() / Tilemap.Tilesize);
            int rightTile = (int)((target.Right() - 1) / Tilemap.Tilesize);
            int topTile = (int)(target.Top() / Tilemap.Tilesize);
            int bottomTile = (int)((target.Bottom() - 1) / Tilemap.Tilesize);
        
            for (int x = leftTile; x <= rightTile; x++)
            {
                for (int y = topTile; y <= bottomTile; y++)
                {
                    tiles.Add(new System.Drawing.Point(x, y));
                }
            }
        
            return tiles;
        }

        public void SetBounds()
        {
            minPos = Vector2.Zero;
            maxPos = Game.tilemap.Mapsize.ToVector2();
            maxPos.X -= Drect.Width;
            maxPos.Y -= Drect.Height;
        }
    }
}