using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;


namespace BlobGame
{
    public class Fireball : CollMoveableSprite
    {
        int fireCounter;
        int fireActiveFrame;
        public static Texture2D[] fireTextures;
        private static SoundEffect explosionSound;
        public bool alive = true;

        public Fireball(Texture2D texture, Rectangle drect, Rectangle srect, GraphicsDeviceManager graphics, bool fireIsLeft) : base(texture, drect, srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Graphics = graphics;
            Velocity = new();
            isLeft = fireIsLeft;
        }

        public override void LoadContent(Game game)
        {
            explosionSound = game.Content.Load<SoundEffect>("assets/sounds/explosion");

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

            if(isLeft)
            {
                Velocity.X = -5;
            } else if(!isLeft)
            {
                Velocity.X = 5;
            }

            Drect.X += (int)Velocity.X;
            horizontalCollisions = GetIntersectingTiles(Drect);
            foreach (var tile in horizontalCollisions)
            {
                if (Tilemap.Collision[(int)Tilemap.level].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if(value == 1 || value == 4 || value == 5 || value >= 16)
                    {
                        //! Do nothing
                    } else if(value == 6)
                    {
                        if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            Tilemap.excludedNormalTiles.Add(new Vector3(29, tile.X, tile.Y));
                            Tilemap.excludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                            alive = false;
                            explosionSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                        }
                    } else {
                        Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.X > 0) // Moving Right
                        {
                            Drect.X = collision.Left - Drect.Width;
                            Velocity.X = 0;
                            alive = false;
                            explosionSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                        }
                        else if (Velocity.X < 0) // Moving Left
                        {
                            Drect.X = collision.Right;
                            Velocity.X = 0;
                            alive = false;
                            explosionSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                        }      
                    }
                }
            }
            
            // Vertical Collision Resolution
            Drect.Y += (int)Velocity.Y;
            verticalCollisions = GetIntersectingTiles(Drect);
            foreach (var tile in verticalCollisions)
            {
                if (Tilemap.Collision[(int)Tilemap.level].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if(value == 1 || value == 4 || value == 5)
                    {
                        //! Do nothing
                    } else if(value == 6)
                    {
                        if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                        {
                            Tilemap.excludedNormalTiles.Add(new Vector3(29, tile.X, tile.Y));
                            Tilemap.excludedCollisionTiles.Add(new Vector3(value, tile.X, tile.Y));
                            alive = false;
                            explosionSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                        }
                    } else {
                        Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                        if (Velocity.Y > 0) // Falling Down
                        {
                            Drect.Y = collision.Top - Drect.Height;
                            Velocity.Y = 0;
                            alive = false;
                            explosionSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                        }
                        else if (Velocity.Y < 0) // Moving Up
                        {
                            Drect.Y = collision.Bottom;
                            Velocity.Y = 0;
                            alive = false;
                            explosionSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                        }
                    }
                }
            }

            if(Drect.X > 3000 || Drect.X < -500 || Drect.Y > 1500 || Drect.Y < -500)
            {
                alive = false;
            }

            if(!alive)
            {
                Main.fireballs.Remove(this);
                Main.sprites.Remove(this);
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

            if (Main.hasF3On)
            {
                foreach (var rect in horizontalCollisions)
                {
                    Main.DrawRectHollow(Globals.SpriteBatch, new Rectangle(rect.X * Tilemap.Tilesize, rect.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize), 1, Color.DarkBlue);
                }
                foreach (var rect in verticalCollisions)
                {
                    Main.DrawRectHollow(Globals.SpriteBatch, new Rectangle(rect.X * Tilemap.Tilesize, rect.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize), 1, Color.DarkBlue);
                }
                Main.DrawRectHollow(Globals.SpriteBatch, Drect, 4, Color.Blue);
            }
        }

        public override string[] GetDebugInfo()
        {
            return new string[] 
            {
                "---FIREBALL---",
                "Velocity: " + Velocity,
                "Is Alive: " + alive,
            };
        }
    }
}