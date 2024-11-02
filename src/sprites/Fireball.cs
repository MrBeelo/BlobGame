using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;


namespace BlobGame
{
    public class Fireball : MoveableSprite
    {
        int fireCounter;
        int fireActiveFrame;
        public static Texture2D[] fireTextures;
        public bool FireIsLeft;
        public bool FireIsAlive = true;
        public List<Point> horizontalCollisions;
        public List<Point> verticalCollisions;

        public Fireball(Texture2D texture, Rectangle drect, Rectangle srect, GraphicsDeviceManager graphics, bool fireIsLeft) : base(texture, drect, srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Graphics = graphics;
            Velocity = new();
            FireIsLeft = fireIsLeft;
        }

        public override void LoadContent(Game game)
        {
            fireTextures = new Texture2D[3];

            fireTextures[0] = game.Content.Load<Texture2D>("assets/sprites/fireball/Fireball1");
            fireTextures[1] = game.Content.Load<Texture2D>("assets/sprites/fireball/Fireball2");
            fireTextures[2] = game.Content.Load<Texture2D>("assets/sprites/fireball/Fireball3");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            fireCounter++;
            if(fireCounter > 14)
            {
                fireCounter = 0;
                fireActiveFrame++;

                if(fireActiveFrame > fireTextures.Length - 1)
                {
                    fireActiveFrame = 0;
                }
            }

            if(FireIsLeft)
            {
                Velocity.X = -5;
            } else if(!FireIsLeft)
            {
                Velocity.X = 5;
            }

            Drect.X += (int)Velocity.X;
            horizontalCollisions = GetIntersectingTiles(Drect);
            foreach (var tile in horizontalCollisions)
            {
                if (Main.collision[(int)Main.level.X].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    Rectangle collision = new Rectangle(tile.X * Main.tilesize, tile.Y * Main.tilesize, Main.tilesize, Main.tilesize);
            
                    if (Velocity.X > 0) // Moving Right
                    {
                        Drect.X = collision.Left - Drect.Width;
                        Velocity.X = 0;
                        FireIsAlive = false;
                    }
                    else if (Velocity.X < 0) // Moving Left
                    {
                        Drect.X = collision.Right;
                        Velocity.X = 0;
                        FireIsAlive = false;
                    }      
                }
            }
            
            // Vertical Collision Resolution
            Drect.Y += (int)Velocity.Y;
            verticalCollisions = GetIntersectingTiles(Drect);
            foreach (var tile in verticalCollisions)
            {
                if (Main.collision[(int)Main.level.X].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    Rectangle collision = new Rectangle(tile.X * Main.tilesize, tile.Y * Main.tilesize, Main.tilesize, Main.tilesize);
            
                    if (Velocity.Y > 0) // Falling Down
                    {
                        Drect.Y = collision.Top - Drect.Height;
                        Velocity.Y = 0;
                        FireIsAlive = false;
                    }
                    else if (Velocity.Y < 0) // Moving Up
                    {
                        Drect.Y = collision.Bottom;
                        Velocity.Y = 0;
                        FireIsAlive = false;
                    }
                }
            }

            if(Drect.X > 3000 || Drect.X < -500 || Drect.Y > 1500 || Drect.Y < -500)
            {
                FireIsAlive = false;
            }

            if(!FireIsAlive)
            {
                Main.fireballs.Remove(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Globals.SpriteBatch.Draw(
                fireTextures[fireActiveFrame],
                Drect,
                Srect,
                Color.White
            );
        }

        public List<Point> GetIntersectingTiles(Rectangle target)
        {
            List<Point> tiles = new List<Point>();
        
            int leftTile = target.Left / Main.tilesize;
            int rightTile = (target.Right - 1) / Main.tilesize;
            int topTile = target.Top / Main.tilesize;
            int bottomTile = (target.Bottom - 1) / Main.tilesize;
        
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