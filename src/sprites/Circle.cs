using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Threading.Tasks;


namespace BlobGame
{
    public class Circle : CollMoveableSprite
    {
        public static Texture2D[] idleTextures;
        int idleCounter;
        int idleActiveFrame;
        public static Texture2D[] walkingTextures;
        int walkingCounter;
        int walkingActiveFrame;
        public static Texture2D[] jumpingTextures;
        public static int circleSizeW = 42;
        public static int circleSizeH = 64;
        Random random = new Random();
        bool randomBool;
        int switchTick = new Random().Next(1, 101);
        int delay = 0;
        int onGroundDelay = 0;
        bool TriangleIsAlive = true;
        bool stop = false;

        public Circle(Texture2D texture, Rectangle drect, Rectangle srect, GraphicsDeviceManager graphics) : base(texture, drect, srect)
        {
            Texture = texture;
            Drect = drect;
            Srect = srect;
            Graphics = graphics;
            Velocity = new();
        }

        public override void LoadContent(Game game)
        {
            base.LoadContent(game);

            idleTextures = new Texture2D[2];
            walkingTextures = new Texture2D[4];
            jumpingTextures = new Texture2D[2];

            idleTextures[0] = game.Content.Load<Texture2D>("assets/sprites/circle/CircleIdle1");
            idleTextures[1] = game.Content.Load<Texture2D>("assets/sprites/circle/CircleIdle2");

            jumpingTextures[0] = game.Content.Load<Texture2D>("assets/sprites/circle/CircleJump1");
            jumpingTextures[1] = game.Content.Load<Texture2D>("assets/sprites/circle/CircleJump2");

            walkingTextures[0] = game.Content.Load<Texture2D>("assets/sprites/circle/CircleWalk1");
            walkingTextures[1] = game.Content.Load<Texture2D>("assets/sprites/circle/CircleWalk2");
            walkingTextures[2] = game.Content.Load<Texture2D>("assets/sprites/circle/CircleWalk1");
            walkingTextures[3] = game.Content.Load<Texture2D>("assets/sprites/circle/CircleWalk3");

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            idleCounter++;
            if(idleCounter > 29)
            {
                idleCounter = 0;
                idleActiveFrame++;

                if(idleActiveFrame > idleTextures.Length - 1)
                {
                    idleActiveFrame = 0;
                }
            }

            walkingCounter++;
            if(walkingCounter > 44 - (speed * 6))
            {
                walkingCounter = 0;
                walkingActiveFrame++;

                if(walkingActiveFrame > walkingTextures.Length - 1)
                {
                    walkingActiveFrame = 0;
                }
            }

            if(switchTick == 75)
            {
                if(!isLeft)
                {
                    isLeft = true;
                } else {
                    isLeft = false;
                }
            }

            if(!isLeft)
            {
                Velocity.X = speed;
            } else if(isLeft)
            {
                Velocity.X = -speed;
            }

            if(delay > 0)
            {
                delay--;
            }

            if(onGroundDelay > 0)
            {
                onGroundDelay--;
            }

            Velocity.Y += 0.5f;

            Velocity.Y = Math.Min(25.0f, Velocity.Y);

            SetBounds();
            Drect.Location = PointClamp(Drect.Location, minPos, maxPos);
            
            // Horizontal Collision Resolution
            if(!stop){Drect.X += (int)Velocity.X;}
            horizontalCollisions = GetIntersectingTiles(Drect);

            isInAir = true;

            if(onGroundDelay == 0 && !isInAir)
            {
                Velocity.Y = -10;
                onGroundDelay = 300;
            }

            randomBool = random.Next(2) == 1; // Generates 0 or 1, then checks if it equals 1
            
            foreach (var tile in horizontalCollisions)
            {
                if (Tilemap.Collision[Tilemap.level].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                    {
                        if(value == 0 || value == 2 || value == 3 || value == 6 || value == 7) //! Solid
                        {
                        Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                            if (Velocity.X > 0) // Moving Right
                            {
                                Drect.X = collision.Left - Drect.Width;

                                if(randomBool)
                                {
                                    isLeft = true;
                                } else if(!randomBool && delay == 0) {
                                    Velocity.Y = -10;
                                    delay = 40;
                                }
                            }
                            else if (Velocity.X < 0) // Moving Left
                            {
                                Drect.X = collision.Right;

                                if(randomBool)
                                {
                                    isLeft = false;
                                } else if(!randomBool && delay == 0) {
                                    Velocity.Y = -10;
                                    delay = 40;
                                }
                            }
                            Velocity.X = 0; // Stop horizontal movement upon collision
                        }
                        else if (value == 4) //! Water
                        {
                            if(Velocity.X > 1)
                            {
                                Velocity.X = 1;
                            }
                            else if(Velocity.X < -1)
                            {
                                Velocity.X = -1;
                            }
                        }
                    }
                }
            }
            
            // Vertical Collision Resolution
            Drect.Y += (int)Velocity.Y;
            verticalCollisions = GetIntersectingTiles(Drect);

            foreach (var tile in verticalCollisions)
            {
                if (Tilemap.Collision[Tilemap.level].TryGetValue(new Vector2(tile.X, tile.Y), out int value))
                {
                    if(!Tilemap.excludedCollisionTiles.Contains(new Vector3(value, tile.X, tile.Y)))
                    {
                        if(value == 0 || value == 2 || value == 3 || value == 6 || value == 7) //! Solid
                        {
                            Rectangle collision = new Rectangle(tile.X * Tilemap.Tilesize, tile.Y * Tilemap.Tilesize, Tilemap.Tilesize, Tilemap.Tilesize);
            
                            if (Velocity.Y > 0) // Falling Down
                            {
                                Drect.Y = collision.Top - Drect.Height;
                                Velocity.Y = 0.5f;
                                isInAir = false;
                            }
                            else if (Velocity.Y < 0) // Moving Up
                            {
                                Drect.Y = collision.Bottom;
                                Velocity.Y = 0;
                            }
                        }
                        else if (value == 4) //! Water
                        {
                            if(Velocity.Y > 0)
                            {
                                Velocity.Y = 1;
                            }
                            isInAir = true;
                        }
                    }
                }
            }

            if(switchTick > 0)
            {
                switchTick--;
            } else {
                switchTick = 100;
            }

            if(switchTick > 25)
            {
                stop = false;
            } else {
                stop = true;
            }

            if(Drect.X > 3000 || Drect.X < -500 || Drect.Y > 1500 || Drect.Y < -500)
            {
                TriangleIsAlive = false;
            }

            if(Drect.Intersects(Main.player.Drect) && Main.player.Immunity == 0 && !Main.player.Immune && !Main.player.isSanic)
            {
                Player.hitSound.Play((float)Main.LoweredVolume, 0.0f, 0.0f);
                Main.player.Health -= 10;
                Main.player.Immunity = 50;
                Player.Damage(10);
            }

            if(!TriangleIsAlive)
            {
                Main.circles.Remove(this);
                //Main.sprites.Remove(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = isLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            if(isMoving && !isInAir && !stop)
            {
                Globals.SpriteBatch.Draw(
                    walkingTextures[walkingActiveFrame],
                    Drect,
                    Srect,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    spriteEffects,
                    0f
                );
            }

            if(isInAir)
            {
                if(Velocity.Y >= 5 || Velocity.Y <= -5)
                {
                    Globals.SpriteBatch.Draw(
                        jumpingTextures[0],
                        Drect,
                        Srect,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        spriteEffects,
                        0f
                    );
                }
                else
                {
                    Globals.SpriteBatch.Draw(
                        jumpingTextures[1],
                        Drect,
                        Srect,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        spriteEffects,
                        0f
                    );
                }
            }

            if((!isMoving || stop) && !isInAir)
            {
                Globals.SpriteBatch.Draw(
                    idleTextures[idleActiveFrame],
                    Drect,
                    Srect,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    spriteEffects,
                    0f
                );
            }

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
                "---CIRCLE---",
                "Random Bool: " + randomBool,
                "Switch Tick: " + switchTick,
                "Delay: " + delay,
                "Is In Air: " + isInAir,
                "Is Left: " + isLeft,
                "Stopped: " + stop,
                "Drect: " + Drect
            };
        }

        public static void Summon(Vector2 pos)
        {
            Circle circle = new Circle(idleTextures[1], new Rectangle((int)pos.X, (int)pos.Y, circleSizeW, circleSizeH), new Rectangle(0, 0, 20, 30), Globals.Graphics);
            Main.circles.Add(circle);
        }

        public static void ClearAll()
        {
            Main.circles.Clear();
        }
    }
}