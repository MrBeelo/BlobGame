using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlobGame
{
    public class CollMoveableSprite : MoveableSprite
    {
        public List<Point> horizontalCollisions;
        public List<Point> verticalCollisions;
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

        public List<Point> GetIntersectingTiles(Rectangle target)
        {
            List<Point> tiles = new List<Point>();
        
            int leftTile = target.Left / Tilemap.Tilesize;
            int rightTile = (target.Right - 1) / Tilemap.Tilesize;
            int topTile = target.Top / Tilemap.Tilesize;
            int bottomTile = (target.Bottom - 1) / Tilemap.Tilesize;
        
            for (int x = leftTile; x <= rightTile; x++)
            {
                for (int y = topTile; y <= bottomTile; y++)
                {
                    tiles.Add(new Point(x, y));
                }
            }
        
            return tiles;
        }
    }
}